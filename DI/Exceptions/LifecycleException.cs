using System;

namespace DI.Exceptions
{
  public class LifecycleException : FeatureException
  {
    public LifecycleException(Exception exception, IFeature feature) : base(exception, feature)
    {
    }
  }
}