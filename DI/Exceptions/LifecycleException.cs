using System;
using DIFeatures.Public;

namespace DIFeatures.Exceptions
{
  public class LifecycleException : Exception, IFeatureException
  {
    public IFeature Feature { get; }

    public LifecycleException(Exception exception, IFeature feature) : base(nameof(LifecycleException), exception) =>
      Feature = feature;
  }
}