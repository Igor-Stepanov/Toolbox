using System.Collections.Generic;
using System.Linq;
using Gantt.Tasks;

namespace Gantt.Combinations
{
  public static class AllCombinations
  {
    public static IEnumerable<Combination> Of(IReadOnlyList<JiraTask> self, int length)
    {
      var combinationTasks = new int[length];

      var selfIndex = 0;
      var combinationIndex = 0;

      while (combinationIndex >= 0)
      {
        if (selfIndex <= (self.Count + (combinationIndex - length)))
        {
          combinationTasks[combinationIndex] = selfIndex;
          if (combinationIndex == length - 1)
          {
            var tasks = combinationTasks
              .Select(x => self[x])
              .ToArray();
            
            yield return new Combination(tasks);
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
  }
}