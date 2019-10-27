using Gantt.Tasks;
using Gantt.Workers;
using static System.Threading.Interlocked;
using static System.Threading.Tasks.Task;

namespace Gantt.Solutions
{
  public class GantSolutions : IGanttSolutions
  {
    public GantSolution Best => _bestSolution;

    public long Total => _total;
    public long Running => _running;

    private long _total;
    private long _running;
    
    private GantSolution _bestSolution;
    
    private readonly Dev[] _developers;
    private readonly QA[] _qas;
    private readonly JiraTask[] _tasks;

    public GantSolutions(Dev[] developers, QA[] qas, JiraTask[] tasks)
    {
      _developers = developers;
      _qas = qas;
      _tasks = tasks;
    }
    
    public void Calculate(GantSolution solution = null,)
    {
      Increment(ref _total);
      Increment(ref _running);

      Run(() =>
      {
        (solution ?? new GantSolution(this, _developers, _qas, _tasks)).Calculate();
        Decrement(ref _running);
      });
    }
    
    public void Account(GantSolution solution)
    {
      if (solution.Days >= _bestSolution?.Days) 
        return;
      
      GantSolution initial;
      do
      {
        initial = _bestSolution;
        
        if (solution.Days >= initial?.Days)
          break;
      }
      while (initial != CompareExchange(ref _bestSolution, solution, initial));
    }
  }
}