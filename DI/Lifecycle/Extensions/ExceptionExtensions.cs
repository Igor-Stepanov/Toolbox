using System;
using DI.Exceptions;

namespace DI.Lifecycle.Extensions
{
  public static class ExceptionExtensions
  {
    public static LifecycleException AsLifecycleException(this Exception self, IFeature feature) =>
      new LifecycleException(self, feature);
  }
}