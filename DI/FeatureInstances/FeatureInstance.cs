using System;
using static System.Activator;

namespace DI.FeatureInstances
{
  internal class FeatureInstance : IFeatureInstance
  {
    public object Value { get; }

    public FeatureInstance(Type type) =>
      Value = CreateInstance(type);
  }
}