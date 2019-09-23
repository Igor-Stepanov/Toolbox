using System.Collections.Generic;
using Gantt.Combinations;
using Gantt.Tasks;
using Gantt.Workers;

namespace Gantt.Solutions.Extensions
{
  public static class CombinationExtensions
  {
    public static bool AssignTo(ref this Combination? self, IReadOnlyList<Worker> freeWorkers, Dictionary<string, JiraTask> tasks)
    {
      if (self == null)
        return false;
      
      for (var i = 0; i < freeWorkers.Count; i++) 
        freeWorkers[i].Assign(tasks[self.Value.Tasks[i].Name]);

      self = null;
      return true;
    }
  }
}