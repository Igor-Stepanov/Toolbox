using System;
using System.Collections.Generic;
using System.Linq;

namespace GantFormula.Extensions
{
  public static class JiraTaskExtensions
  {
    public static IEnumerable<Combination> CombinationsOf(this List<JiraTask> self, int combinationLength)
    {
      var combinationTasks = new int[combinationLength];

      var selfIndex = 0;
      var combinationIndex = 0;

      while (combinationIndex >= 0)
      {
        if (selfIndex <= (self.Count + (combinationIndex - combinationLength)))
        {
          combinationTasks[combinationIndex] = selfIndex;
          if (combinationIndex == combinationLength - 1)
          {
            var combination = new Combination();
            foreach (var taskIndex in combinationTasks)
              combination.With(self[taskIndex]);
            
            yield return combination;
            
            selfIndex++;
          }
          else
          {
            // if combination is not full yet, select next element
            selfIndex = combinationTasks[combinationIndex] + 1;
            combinationIndex++;
          }
        }

        // backward step
        else
        {
          if (--combinationIndex >= 0)
            selfIndex = combinationTasks[combinationIndex] + 1;
        }
      }
    }


    // ReSharper disable PossibleMultipleEnumeration
    public static IEnumerable<Combination> Combinations(this IEnumerable<JiraTask> tasks, int by)
    {
      if (by == 1)
        return tasks.Select(x => new Combination(x));

      return Combinations(tasks, by - 1)
        .SelectMany(x => tasks.Where(t => t != x.Last), (combination, task) => combination.With(task))
        .Distinct();
    }

    public class Combination : IEquatable<Combination>
    {
      public JiraTask Last => _tasks.Last();
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
          : _tasks.Sum(x => x.DevelopmentDays + x.QaDays).GetHashCode();

      public override string ToString() =>
        $"{string.Join("\r\n", _tasks)}";
    }

    private static IEnumerable<JiraTask> Yielded(this JiraTask self)
    {
      yield return self;
    }
  }
}