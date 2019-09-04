using System;

namespace DI.Exceptions
{
  public abstract class FeatureException : Exception
  {
    private readonly IFeature _feature;

    public FeatureException(Exception exception, IFeature feature) : base()
    {
      _feature = feature;
    }
  }
}