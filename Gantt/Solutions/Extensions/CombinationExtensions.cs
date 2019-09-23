using System.Collections.Generic;
using Gantt.Combinations;
using Gantt.Tasks;
using Gantt.Workers;

namespace Gantt.Solutions.Extensions
{
  public static class CombinationExtensions
  {
    public static void AssignTo(this Combination self, IReadOnlyList<Worker> freeWorkers, Dictionary<string, JiraTask> tasks)
    {
      for (var i = 0; i < freeWorkers.Count; i++)
        freeWorkers[i].Assign(tasks[self.Tasks[i]]);
    }
  }
}