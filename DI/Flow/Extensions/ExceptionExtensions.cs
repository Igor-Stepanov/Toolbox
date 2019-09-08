using System;
using DIFeatures.Exceptions;
using DIFeatures.Public;

namespace DIFeatures.Flow.Extensions
{
  public static class ExceptionExtensions
  {
    public static LifecycleException AsLifecycleException(this Exception self, IFeature feature) =>
      new LifecycleException(self, feature);
  }
}