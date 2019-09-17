using System;
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
    [Key(0)] public DateTime Day;
    
    [Key(1)] public List<Developer> Developers;
    [Key(2)] public List<Qa> Qas;
    [Key(3)] public List<JiraTask> Tasks;
    
    [IgnoreMember] public int Id { get; set; }
    [IgnoreMember] public IGantSolutions Solutions { get; set; }

    [IgnoreMember] public Combination DevCombination { get; set; }
    [IgnoreMember] public Combination QaCombination { get; set; }

    [IgnoreMember] private IReadOnlyList<Worker> FreeDevs => Developers.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] private IReadOnlyList<Worker> FreeQas => Qas.Where(x => x.Task == null).Cast<Worker>().ToList();

    [IgnoreMember] private List<JiraTask> FreeDevTasks => Tasks
      .Where(x => !x.DevDone)
      .ToList();
    
    [IgnoreMember] private List<JiraTask> ReadyForQaTasks => Tasks
      .Where(x => !x.QaDone)
      .ToList();

    public GantSolution() { }

    public GantSolution(DateTime day, List<Developer> developers, List<Qa> qas, List<JiraTask> tasks)
    {
      Day = day;
      
      Developers = developers;
      Qas = qas;
      Tasks = tasks;
    }

    public void Calculate()
    {
      while (Tasks.Any(x => !x.Complete))
      {
        if (DevCombination != null)
        {
          AssignPredefined(DevCombination, FreeDevs);
          DevCombination = null;
        }
        else if (!Assign(FreeDevTasks, FreeDevs, out var alternativeDevCombinations))
        {
          alternativeDevCombinations
            .Skip(1)
            .ForEach(x => Solutions.Continue(this, devCombination: x));
          
          AssignPredefined(alternativeDevCombinations.First(), FreeDevs);
        }

        if (QaCombination != null)
        {
          AssignPredefined(QaCombination, FreeQas);
          QaCombination = null;
        }
        else if (!Assign(ReadyForQaTasks, FreeQas, out var alternativeQaCombinations))
        {
          alternativeQaCombinations
            .Skip(1)
            .ForEach(x => Solutions.Continue(this, qaCombination: x));
          
          AssignPredefined(alternativeQaCombinations.First(), FreeQas);
        }

        Work();

        Day.Next();
      }
    }

    private static bool Assign(IReadOnlyList<JiraTask> freeTasks, IReadOnlyList<Worker> freeWorkers, out List<Combination> combinations)
    {
      combinations = null;
      
      if(!freeTasks.Any() || !freeWorkers.Any())
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
    
    private static void AssignPredefined(Combination combination, IReadOnlyList<Worker> freeWorkers)
    {
      for (var i = 0; i < freeWorkers.Count; i++) 
        freeWorkers[i].Assign(combination.Tasks[i]);
    }

    private void Work() =>
      Developers
        .Cast<Worker>()
        .Concat(Qas)
        .ForEach(x => x.Work(Day, Tasks));
  }
}