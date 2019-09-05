using System;
using DI.Exceptions;

namespace DI.Lifecycles.Extensions
{
  public static class ExceptionExtensions
  {
    public static LifecycleException AsLifecycleException(this Exception self) =>
      new LifecycleException(self);
    
    public static LifecycleException AsFeatureLifecycleException(this Exception self, IFeature feature) =>
      new FeatureLifecycleException(self, feature);

  }
}