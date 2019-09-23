using System.Collections.Generic;
using System.Linq;
using Gantt.Combinations;
using Gantt.Solutions.Extensions;
using Gantt.Tasks;
using Gantt.Workers;
using MessagePack;
using static Gantt.Jobs.JobStatus;

namespace Gantt.Solutions
{
  [MessagePackObject]
  public class GantSolution
  {
    [Key(0)] public int Days;
    [Key(1)] public Developer[] Devs;
    [Key(2)] public QA[] QAs;
    [Key(3)] public JiraTask[] Tasks;

    [IgnoreMember] private Dictionary<string, JiraTask> _tasks;
    [IgnoreMember] private Dictionary<string, JiraTask> TasksDictionary =>
      _tasks ?? (_tasks = Tasks.ToDictionary(x => x.Name, x => x)); 
    
    [IgnoreMember] public int Id { get; set; }
    [IgnoreMember] public IGanttSolutions Solutions { get; set; }

    [IgnoreMember] public Combination? Combination;

    [IgnoreMember] private IReadOnlyList<Worker> FreeDevs => Devs.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] private IReadOnlyList<Worker> FreeQAs => QAs.Where(x => x.Task == null).Cast<Worker>().ToList();
    [IgnoreMember] public IEnumerable<Worker> Workers => Devs.Cast<Worker>().Concat(QAs);

    [IgnoreMember] private List<JiraTask> FreeDevTasks => Tasks
     .Where(x => x.Assignee == null && x.DevJob.Status == Free)
     .ToList();

    [IgnoreMember] private List<JiraTask> FreeQATasks => Tasks
     .Where(x => x.Assignee == null && x.DevJob.Status == Done && x.QAJob.Status == Free)
     .ToList();

    public GantSolution(){}
    public GantSolution(Developer[] devs, QA[] qas, JiraTask[] tasks) =>
      (Devs, QAs, Tasks) =
      (devs, qas, tasks);

    public void Calculate()
    {
      while (Tasks.Any(x => !x.Complete))
      {
        if (++Days >= Solutions.Best?.Days)
          return;

        Assign(FreeDevs, FreeDevTasks);
        Assign(FreeQAs, FreeQATasks);

        Work();
      }

      Solutions.Add(this);
    }

    private void Assign(IReadOnlyList<Worker> freeWorkers, IReadOnlyList<JiraTask> freeTasks)
    {
      if(freeWorkers.Count == 0 || freeTasks.Count == 0)
        return;
      
      if (freeWorkers.Count >= freeTasks.Count)
      {
        for (var i = 0; i < freeTasks.Count; i++) 
          freeWorkers[i].Assign(freeTasks[i]);
        
        return;
      }

      if (Combination != null)
      {
        Combination.Value.AssignTo(freeWorkers, TasksDictionary);
        Combination = null;
        return;
      }

      foreach (var combination in AllCombinations.Of(freeTasks, freeWorkers.Count))
      {
        if (Combination == null)
        {
          Combination = combination;
          continue;
        }
        
        Solutions.Continue(this, combination);
      }

      Combination?.AssignTo(freeWorkers, TasksDictionary);
      Combination = null;
    }

    private void Work()
    {
      for (var i = 0; i < Devs.Length; i++)
        Devs[i].Work(Days, TasksDictionary);
      
      for (var i = 0; i < QAs.Length; i++)
        QAs[i].Work(Days, TasksDictionary);
    }
  }
}