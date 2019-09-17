using System;
using System.Collections.Generic;
using System.Linq;

namespace GantFormula.Extensions
{
  public class Combination : IEquatable<Combination>
  {
    public JiraTask Last => _tasks.Last();
    public IReadOnlyList<JiraTask> Tasks => _tasks;
    
    private readonly List<JiraTask> _tasks;

    public Combination() =>
      _tasks = new List<JiraTask>();
      
    public Combination(JiraTask task) =>
      _tasks = new List<JiraTask> {task};

    public Combination(IEnumerable<JiraTask> tasks) =>
      _tasks = tasks.ToList();

    public Combination With(JiraTask task)
    {
      _tasks.Add(task);
      return this;
    }

    public bool Equals(Combination other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;

      var otherList = other._tasks.ToList();

      foreach (var task in _tasks)
      {
        var otherTask = otherList.FirstOrDefault(x => x.Equals(task));
        if (otherTask == null)
          return false;

        otherList.Remove(otherTask);
      }

      return otherList.Count == 0;
    }
      
    public override int GetHashCode() =>
      _tasks == null
        ? 0
        : _tasks.Sum(x => x.DevDays + x.QaDays).GetHashCode();

    public override string ToString() =>
      $"{String.Join("\r\n", _tasks)}";
  }
}