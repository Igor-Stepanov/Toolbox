using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using GantFormula.Extensions;
using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public class GantSolution
  {
    [Key(0)] public int Days;
    
    [Key(1)] public Developer[] Devs;
    [Key(2)] public Qa[] Qas;
    [Key(3)] public Dictionary<string, JiraTask> Tasks;
    
    [IgnoreMember] public int Id { get; set; }
    [IgnoreMember] public IGant Gant { get; set; }

    [IgnoreMember] public Combination DevCombination { get; set; }
    [IgnoreMember] public Combination QaCombination { get; set; }

    [IgnoreMember] private IReadOnlyList<Worker> FreeDevs => Devs.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] private IReadOnlyList<Worker> FreeQas => Qas.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] public IEnumerable<Worker> Workers => Devs.Cast<Worker>().Union(Qas);

    [IgnoreMember] private List<JiraTask> FreeDevTasks => Tasks
     .Values
     .Where(x => x.Assignee == null && x.DevProgress == 0)
     .ToList();

    [IgnoreMember] private List<JiraTask> ReadyForQaTasks => Tasks
     .Values
     .Where(x => x.Assignee == null && x.DevDone && x.QaProgress == 0 )
     .ToList();

    public GantSolution() { }

    public GantSolution(Developer[] devs, Qa[] qas, Dictionary<string, JiraTask> tasks) =>
      (Devs, Qas, Tasks) =
      (devs, qas, tasks);

    public void Calculate()
    {
      while (Tasks.Values.Any(x => !x.Complete))
      {
        if (++Days >= Gant.Solution?.Days)
          return;
        
        var freeDevs = FreeDevs;

        if (DevCombination != null)
        {
          AssignPredefined(DevCombination, freeDevs);
          DevCombination = null;
        }
        else if (!Assign(FreeDevTasks, freeDevs, out var alternativeDevCombinations))
        {
          alternativeDevCombinations
            .Skip(1)
            .ForEach(x => Gant.Continue(this, devCombination: x));
          
          AssignPredefined(alternativeDevCombinations.First(), freeDevs);
        }

        var freeQas = FreeQas;

        if (QaCombination != null)
        {
          AssignPredefined(QaCombination, freeQas);
          QaCombination = null;
        }
        else if (!Assign(ReadyForQaTasks, freeQas, out var alternativeQaCombinations))
        {
          alternativeQaCombinations
            .Skip(1)
            .ForEach(x => Gant.Continue(this, qaCombination: x));
          
          AssignPredefined(alternativeQaCombinations.First(), freeQas);
        }

        Work();
      }

      Gant.Add(this);
    }

    private static bool Assign(IReadOnlyList<JiraTask> freeTasks, IReadOnlyList<Worker> freeWorkers, out List<Combination> combinations)
    {
      combinations = null;
      
      if(freeTasks.Count == 0 || freeWorkers.Count == 0)
        return true;
      
      if (freeWorkers.Count >= freeTasks.Count)
      {
        for (var i = 0; i < freeTasks.Count; i++) 
          freeWorkers[i].Assign(freeTasks[i]);
        
        return true;
      }

      combinations = freeTasks.CombinationsOf(freeWorkers.Count).ToList();
      return false;
    }
    
    private void AssignPredefined(Combination combination, IReadOnlyList<Worker> freeWorkers)
    {
      var workers = freeWorkers.GetEnumerator();
      
      for (var i = 0; i < freeWorkers.Count; i++) 
        freeWorkers[i].Assign(Tasks[combination.Tasks[i].Name]);
    }

    private void Work()
    {
      for (var i = 0; i < Devs.Count; i++)
        Devs[i].Work(Days, Tasks);
      
      for (var i = 0; i < Qas.Count; i++)
        Qas[i].Work(Days, Tasks);
    }
  }
}