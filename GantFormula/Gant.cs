using System.Collections.Generic;
using System.Threading;
using GantFormula.Extensions;
using static System.Threading.Tasks.Task;

namespace GantFormula
{
  public class Gant : IGant
  {
    public GantSolution Solution => _solution;

    public long TotalTasks;
    public long RunningTasks;

    private readonly List<Developer> _developers;
    private readonly List<Qa> _qas;
    private readonly List<JiraTask> _tasks;

    private GantSolution _solution;
    private int _nextId = 1;

    public Gant(List<Developer> developers, List<Qa> qas, List<JiraTask> tasks)
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
            Gant = this,
          }.Calculate();

          Interlocked.Decrement(ref RunningTasks);
        }
      );
    }
    
    public void Continue(GantSolution solution, Combination devCombination = null, Combination qaCombination = null)
    {
      var solutionVariant = solution.Copy();
      
      solutionVariant.Id = Interlocked.Increment(ref _nextId);
      solutionVariant.Gant = this;
      
      solutionVariant.DevCombination = devCombination;
      solutionVariant.QaCombination = qaCombination;

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