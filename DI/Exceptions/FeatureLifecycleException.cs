using System;
using FeaturesDI.Client;

namespace FeaturesDI.Exceptions
{
  public class FeatureLifecycleException : LifecycleException, IFeatureException
  {
    public IFeature Feature { get; }

    public FeatureLifecycleException(Exception exception, IFeature feature): base(exception) => 
      Feature = feature;
  }
}