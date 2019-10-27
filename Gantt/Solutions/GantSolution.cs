using System.Collections.Generic;
using System.Linq;
using Gantt.Combinations;
using Gantt.Solutions.Extensions;
using Gantt.Tasks;
using Gantt.Workers;
using static Gantt.Jobs.JobStatus;

namespace Gantt.Solutions
{
  public class GantSolution
  {
    public int Days { get; private set; }
    
    private readonly Dev[] _devs;
    private readonly QA[] _qas;
    private readonly JiraTask[] _tasks;
    private readonly IGanttSolutions _solutions;
    
    private Combination? _combination;

    private Dictionary<string, JiraTask> _tasksCache;
    private Dictionary<string, JiraTask> TasksCache =>
      _tasksCache ?? (_tasksCache = _tasks.ToDictionary(x => x.Name, x => x));

    private IReadOnlyList<Worker> FreeDevs => _devs.Where(x => x.Task == null).Cast<Worker>().ToList();
    private IReadOnlyList<Worker> FreeQAs => _qas.Where(x => x.Task == null).Cast<Worker>().ToList();
    public IEnumerable<Worker> Workers => _devs.Cast<Worker>().Concat(_qas);

    private List<JiraTask> FreeDevTasks => _tasks
     .Where(x => x.Assignee == null && x.DevJob.Status == Free)
     .ToList();

    private List<JiraTask> FreeQATasks => _tasks
     .Where(x => x.Assignee == null && x.DevJob.Status == Done && x.QAJob.Status == Free)
     .ToList();

    private GantSolution(IGanttSolutions solutions, int days, Dev[] devs, QA[] qas, JiraTask[] tasks)
      : this(solutions, devs, qas, tasks) =>
      Days = days;

    public GantSolution(IGanttSolutions solutions, Dev[] devs, QA[] qas, JiraTask[] tasks)
    {
      _solutions = solutions;
      
      _devs = devs.CloneAll();
      _qas = qas.CloneAll();
      _tasks = tasks.CloneAll();
    }

    public void Calculate()
    {
      while (_tasks.Any(x => !x.Complete))
      {
        if (++Days >= _solutions.Best?.Days)
          return;

        Assign(FreeDevs, FreeDevTasks);
        Assign(FreeQAs, FreeQATasks);

        Work();
      }

      _solutions.Account(this);
    }

    public GantSolution Clone(Combination with) =>
      new GantSolution(_solutions, Days, _devs, _qas, _tasks);

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

      if (_combination != null)
      {
        _combination.Value.AssignTo(freeWorkers, TasksCache);
        _combination = null;
        return;
      }

      foreach (var combination in AllCombinations.Of(freeTasks, freeWorkers.Count))
      {
        if (_combination == null)
        {
          _combination = combination;
          continue;
        }
        
        _solutions.Calculate(this, combination);
      }

      _combination?.AssignTo(freeWorkers, TasksCache);
      _combination = null;
    }

    private void Work()
    {
      for (var i = 0; i < _devs.Length; i++)
        _devs[i].Work(Days, TasksCache);
      
      for (var i = 0; i < _qas.Length; i++)
        _qas[i].Work(Days, TasksCache);
    }
  }
}