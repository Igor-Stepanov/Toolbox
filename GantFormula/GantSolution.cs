using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using GantFormula.Extensions;
using static GantFormula.Status;

namespace GantFormula
{
  public class GantSolution
  {
    private DateTime _day;
    
    private readonly List<Developer> _developers;
    private readonly List<Qa> _qas;
    private readonly List<JiraTask> _tasks;
    
    private readonly IGantSolutions _solutions;

    public GantSolution(IGantSolutions solutions, GantSolutionStage stage)
    {
      _day = stage.Day;
      
      _developers = stage.Developers;
      _qas = stage.Qas;
      _tasks = stage.Tasks;
      
      _solutions = solutions;
    }

    public void Calculate()
    {
      while (_tasks.Any(x => x.Status != Done))
      {
        Assign(UnassignedTasks, _developers.Where(x => x.Free));
        Assign(ReadyForQaTasks, _qas.Where(x => x.Free));
        
        Work(_day, _developers, _qas);

        _day.Next();
      }
      
      _solutions.Register(this);
    }

    private void Assign(List<JiraTask> unassignedTasks, IEnumerable<IWorker> freeWorkers)
    {
      if(!unassignedTasks.Any())
        return;
      
      var freeWorkersList = freeWorkers.ToList();
      if (freeWorkersList.Count >= unassignedTasks.Count)
      {
        for (var i = 0; i < unassignedTasks.Count; i++) 
          freeWorkersList[i].Assign(unassignedTasks[i]);
        
        return;
      }
      var combinations = unassignedTasks.CombinationsOf(freeWorkersList.Count);
      var first = combinations.First();
      
    }

    private void Work(DateTime day, params IEnumerable<IWorker>[] workers) =>
      workers
        .SelectMany(x => x)
        .ForEach(x => x.Work(day));

    private List<JiraTask> UnassignedTasks =>
      _tasks
        .Where(x => x.Status == Unassigned)
        .ToList();

    private List<JiraTask> ReadyForQaTasks => 
      _tasks
        .Where(x => x.Status == ReadyForQa)
        .ToList();
  }

  public struct GantSolutionStage
  {
    public DateTime Day { get; set; }
    public List<Developer> Developers { get; set; }
    public List<Qa> Qas { get; set; }
    public List<JiraTask> Tasks { get; set; }

    public List<JiraTaskExtensions.Combination> DevelopersNextWork { get; set; } // Empty 
    public List<JiraTaskExtensions.Combination> QasNextWork { get; set; }
  }
}