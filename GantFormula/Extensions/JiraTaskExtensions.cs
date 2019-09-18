using System.Collections.Generic;
using System.Linq;

namespace GantFormula.Extensions
{
  public static class JiraTaskExtensions
  {
    public static IEnumerable<Combination> CombinationsOf(this IReadOnlyList<JiraTask> self, int combinationLength)
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
            selfIndex = combinationTasks[combinationIndex] + 1;
            combinationIndex++;
          }
        }
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

    private static IEnumerable<JiraTask> Yielded(this JiraTask self)
    {
      yield return self;
    }
  }
}