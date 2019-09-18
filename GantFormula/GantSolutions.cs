using System;
using System.Collections.Generic;
using System.Linq;
using GantFormula.Extensions;

namespace GantFormula
{
  public class GantSolutions : IGantSolutions
  {
    public IEnumerable<GantSolution> All => _solutions;

    private readonly List<Developer> _developers;
    private readonly List<Qa> _qas;
    private readonly List<JiraTask> _tasks;

    private readonly List<GantSolution> _solutions;
    private int NextId => _solutions.LastOrDefault()?.Id + 1 ?? 1; 

    public GantSolutions(List<Developer> developers, List<Qa> qas, List<JiraTask> tasks)
    {
      _solutions = new List<GantSolution>();
      
      _developers = developers;
      _qas = qas;
      _tasks = tasks;
    }
    
    public void Calculate()
    {
      var solution = new GantSolution(1, _developers, _qas, _tasks)
        {
          Id = NextId,
          Solutions = this,
        };
        
        _solutions.Add(solution);
        solution.Calculate();
    }
    
    public void Continue(GantSolution solution, Combination devCombination = null, Combination qaCombination = null)
    {
      var solutionVariant = solution.Copy();
      
      solutionVariant.Id = NextId;
      solutionVariant.Solutions = this;
      
      solutionVariant.DevCombination = devCombination;
      solutionVariant.QaCombination = qaCombination;
      
      _solutions.Add(solutionVariant);
      solutionVariant.Calculate();
    }
  }
}