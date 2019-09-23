using System;
using System.Linq;
using Common.Hashes;
using Gantt.Combinations.Complexity;
using Gantt.Tasks;
using static System.String;

namespace Gantt.Combinations
{
  public struct Combination : IEquatable<Combination>
  { 
    public readonly string[] Tasks;
    private readonly CombinationComplexity _complexity;
    
    public Combination(JiraTask[] tasks)
    {
      _complexity = CombinationComplexity.Of(tasks);
      Tasks = tasks
        .Select(x => x.Name)
        .ToArray();
    }

    public bool Equals(Combination other) => 
      _complexity.Equals(other._complexity);

    public override int GetHashCode() =>
      Hash.Of(Tasks);

    public override string ToString() =>
      $"{Join("\r\n", Tasks.Cast<object>())}";
  }
}