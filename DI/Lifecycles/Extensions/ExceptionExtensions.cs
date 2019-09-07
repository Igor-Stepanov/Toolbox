using System;
using FeaturesDI.Client;
using FeaturesDI.Exceptions;

namespace FeaturesDI.Lifecycles.Extensions
{
  public static class ExceptionExtensions
  {
    public static LifecycleException AsLifecycleException(this Exception self, IFeature feature) =>
      new LifecycleException(self, feature);
  }
}