using System;
using DependencyInjection.API.Dependencies;

namespace DependencyInjection.Model.Exceptions
{
  internal class FeatureException : Exception
  {
    public Feature Feature { get; }

    public FeatureException(Exception exception, Feature feature) 
      : base(feature.ToString(), exception) =>
      Feature = feature;
  }
  
  internal static class ExceptionExtensions
  {
    public static Exception AsFeatureException(this Exception self, Feature feature) =>
      new FeatureException(self, feature);
  }
}