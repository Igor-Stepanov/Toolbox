using System.Collections.Generic;
using System.Linq;
using Gantt.Extensions;
using MessagePack;
using static Gantt.Jobs.JobStatus;

namespace Gantt.Solutions
{
  [MessagePackObject]
  public class GantSolution
  {
    [Key(0)] public int Days;
    
    [Key(1)] public Developer[] Devs;
    [Key(2)] public Qa[] Qas;
    [Key(3)] public JiraTask[] Tasks;

    [IgnoreMember] private Dictionary<string, JiraTask> _tasks;
    [IgnoreMember] private Dictionary<string, JiraTask> TasksDictionary =>
      _tasks ?? (_tasks = Tasks.ToDictionary(x => x.Name, x => x)); 
    
    [IgnoreMember] public int Id { get; set; }
    [IgnoreMember] public IGanttSolutions Solutions { get; set; }
    
    [IgnoreMember] public Combination Combination { get; set; }

    [IgnoreMember] private IReadOnlyList<Worker> FreeDevs => Devs.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] private IReadOnlyList<Worker> FreeQas => Qas.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] public IEnumerable<Worker> Workers => Devs.Cast<Worker>().Union(Qas);

    [IgnoreMember] private List<JiraTask> FreeDevTasks => Tasks
     .Where(x => x.Assignee == null && x.DevJob.Status == Free)
     .ToList();

    [IgnoreMember] private List<JiraTask> FreeQaTasks => Tasks
     .Where(x => x.Assignee == null && x.DevJob.Status == Done && x.QaJob.Status == Free)
     .ToList();

    public GantSolution() { }

    public GantSolution(Developer[] devs, Qa[] qas, JiraTask[] tasks) =>
      (Devs, Qas, Tasks) =
      (devs, qas, tasks);

    public void Calculate()
    {
      while (Tasks.Any(x => !x.Complete))
      {
        if (++Days >= Solutions.Best?.Days)
          return;

        Assign(FreeDevTasks, FreeDevs);
        Assign(FreeQaTasks, FreeQas);

        Work();
      }

      Solutions.Add(this);
    }

    private void Assign(IReadOnlyList<JiraTask> freeTasks, IReadOnlyList<Worker> freeWorkers)
    {
      if(freeTasks.Count == 0 || freeWorkers.Count == 0)
        return;
      
      if (freeWorkers.Count >= freeTasks.Count)
      {
        for (var i = 0; i < freeTasks.Count; i++) 
          freeWorkers[i].Assign(freeTasks[i]);
        
        return;
      }

      if (Combination != null)
      {
        Assign(Combination, freeWorkers);
        Combination = null;
        return;
      }

      var combinations = freeTasks.CombinationsOf(freeWorkers.Count).ToArray();
      
      for (var i = 0; i < combinations.Length; i++)
        Solutions.Continue(this, combinations[i]);
          
      Assign(combinations[0], freeWorkers);
    }
    
    private void Assign(Combination combination, IReadOnlyList<Worker> freeWorkers)
    {
      for (var i = 0; i < freeWorkers.Count; i++) 
        freeWorkers[i].Assign(TasksDictionary[combination.Tasks[i].Name]);
    }

    private void Work()
    {
      for (var i = 0; i < Devs.Length; i++)
        Devs[i].Work(Days, TasksDictionary);
      
      for (var i = 0; i < Qas.Length; i++)
        Qas[i].Work(Days, TasksDictionary);
    }
  }
}