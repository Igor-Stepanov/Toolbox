using Gantt.Tasks;

namespace Gantt.Combinations.Complexity.Extensions
{
  public static class JiraTaskExtensions
  {
    public static TaskComplexity Complexity(this JiraTask self) =>
      new TaskComplexity(self.DevJob.Amount, self.DevJob.Amount);
  }
}