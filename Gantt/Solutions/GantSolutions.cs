using System.Threading;
using Gantt.Combinations;
using Gantt.MessagePack;
using Gantt.Tasks;
using Gantt.Workers;
using static System.Threading.Tasks.Task;

namespace Gantt.Solutions
{
  public class GantSolutions : IGanttSolutions
  {
    public GantSolution Best => _solution;

    public long TotalTasks;
    public long RunningTasks;

    private readonly Developer[] _developers;
    private readonly QA[] _qas;
    private readonly JiraTask[] _tasks;

    private GantSolution _solution;
    private int _nextId = 1;

    public GantSolutions(Developer[] developers, QA[] qas, JiraTask[] tasks)
    {
      _developers = developers;
      _qas = qas;
      _tasks = tasks;
    }
    
    public void Calculate()
    {
      Interlocked.Increment(ref TotalTasks);
      Interlocked.Increment(ref RunningTasks);

      Run(() =>
        {
          new GantSolution(_developers, _qas, _tasks)
          {
            Id = _nextId,
            Solutions = this,
          }.Calculate();

          Interlocked.Decrement(ref RunningTasks);
        }
      );
    }
    
    public void Continue(GantSolution solution, Combination combination)
    {
      var solutionVariant = solution.Copy();
      
      solutionVariant.Id = Interlocked.Increment(ref _nextId);
      
      solutionVariant.Solutions = this;
      solutionVariant.Combination = combination;

      Interlocked.Increment(ref TotalTasks);
      Interlocked.Increment(ref RunningTasks);

      Run(() =>
      {
        solutionVariant.Calculate();
        Interlocked.Decrement(ref RunningTasks);
      });
    }

    public void Add(GantSolution solutionVariant)
    {
      if (solutionVariant.Days >= _solution?.Days) 
        return;
      
      GantSolution initial;
      do
      {
        initial = _solution;
        
        if (solutionVariant.Days >= initial?.Days)
          break;
      }
      while (initial != Interlocked.CompareExchange(ref _solution, solutionVariant, initial));
    }
  }
}