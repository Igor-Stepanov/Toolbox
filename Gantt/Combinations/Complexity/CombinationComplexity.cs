using System;
using Common.Extensions;
using Common.Hashes;
using Gantt.Combinations.Complexity.Extensions;
using Gantt.Tasks;

namespace Gantt.Combinations.Complexity
{
  public struct CombinationComplexity : IEquatable<CombinationComplexity>
  {
    private readonly TaskComplexity[] _complexities;

    private CombinationComplexity(JiraTask[] tasks)
    {
      _complexities = new TaskComplexity[tasks.Length];
      for (var i = 0; i < tasks.Length; i++)
        _complexities[i] = tasks[i].Complexity();
    }

    public static CombinationComplexity Of(JiraTask[] tasks) => 
      new CombinationComplexity(tasks);

    public bool Equals(CombinationComplexity other) =>
      _complexities?.Length == other._complexities?.Length &&
      _complexities.Each(x => x.EqualsAnyOf(other._complexities));

    public override bool Equals(object other) => 
      other is CombinationComplexity complexity && Equals(complexity);

    public override int GetHashCode() =>
      Hash.Of(_complexities);
  }
}