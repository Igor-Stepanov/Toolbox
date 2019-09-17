using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public abstract class Worker
  {
    [Key(0)] public int Id;
    [Key(1)] public string Task;
    [Key(2)] public IEnumerable<WorkDay> WorkDays;

    protected Worker() { }
    protected Worker(int id)
    {
      Id = id;
      WorkDays = new List<WorkDay>();
    }

    public void Assign(JiraTask task) => 
      Task = task.Name;

    public void Work(DateTime day, IEnumerable<JiraTask> tasks)
    {
      if (Task != null)
      {
        var task = tasks.Single(x => x.Name == Task);
        if (Work(task))
          Task = null;

        WorkDays.As<List<WorkDay>>(x => x.Add(new WorkDay
        {
          Task = Task,
          Day = day
        }));
      }
    }

    protected abstract bool Work(JiraTask task);
  }
}