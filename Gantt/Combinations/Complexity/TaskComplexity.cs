using System;
using Common.Hashes;

namespace Gantt.Combinations.Complexity
{
  public struct TaskComplexity : IEquatable<TaskComplexity>
  {
    public readonly int Dev;
    public readonly int QA;

    public TaskComplexity(int dev, int qa)
    {
      Dev = dev;
      QA = qa;
    }

    public bool Equals(TaskComplexity other) => 
      Dev == other.Dev
      && QA == other.QA;

    public override bool Equals(object obj)
    {
      return obj is TaskComplexity other && Equals(other);
    }

    public override int GetHashCode() =>
      Hash.Of(Dev)
        .With(QA);

  }
}