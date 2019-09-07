using System;
using FeaturesDI.Client;

namespace FeaturesDI.Exceptions
{
  public class LifecycleException : Exception, IFeatureException
  {
    public IFeature Feature { get; }

    public LifecycleException(Exception exception, IFeature feature) : base(nameof(LifecycleException), exception) =>
      Feature = feature;
  }
}