using System;
using FeaturesDI.Client;
using FeaturesDI.Exceptions;

namespace FeaturesDI.Lifecycles.Extensions
{
  public static class ExceptionExtensions
  {
    public static LifecycleException AsLifecycleException(this Exception self) =>
      new LifecycleException(self);
    
    public static LifecycleException AsFeatureLifecycleException(this Exception self, IFeature feature) =>
      new FeatureLifecycleException(self, feature);

  }
}