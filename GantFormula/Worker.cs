using System;
using System.Collections.Generic;

namespace GantFormula
{
  public abstract class Worker : IWorker
  {
    public bool Free =>
      Task == null;

    protected JiraTask Task;

    public IEnumerable<WorkDay> WorkDays => _workDays;
    private readonly List<WorkDay> _workDays = new List<WorkDay>();

    void IWorker.Assign(JiraTask task)
    {
      Task = task;
      Task.Assignee = this;
      Assign(Task);
    }

    void IWorker.Work(DateTime day)
    {
      if (Task != null)
      {
        Work();
        
        _workDays.Add(new WorkDay
        {
          Worker = this,
          Task = Task,
          Day = day
        });
      }
    }

    protected abstract void Work();
    protected abstract void Assign(JiraTask task);
  }
}