using System;
using System.Collections.Generic;

namespace GantFormula
{
  public class GantSolutions : IGantSolutions
  {
    public IEnumerable<GantSolution> All => _solutions;

    private readonly List<Developer> _developers;
    private readonly List<Qa> _qas;
    private readonly List<JiraTask> _tasks;

    private readonly List<GantSolution> _solutions;

    public GantSolutions(List<Developer> developers, List<Qa> qas, List<JiraTask> tasks)
    {
      _solutions = new List<GantSolution>();
      
      _developers = developers;
      _qas = qas;
      _tasks = tasks;
    }

    void IGantSolutions.Register(GantSolution solution) =>
      _solutions.Add(solution);

    public List<GantSolution> Calculate()
    {
      new GantSolution(this, new GantSolutionStage
        {
          Day = DateTime.Now,
          Developers = _developers,
          Qas = _qas,
          Tasks = _tasks
        })
        .Calculate();

      return _solutions;
    }
  }
}