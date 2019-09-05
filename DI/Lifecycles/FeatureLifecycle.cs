using System;
using DI.Exceptions;
using DI.Lifecycles.Extensions;

namespace DI.Lifecycles
{
  public class FeatureLifecycle : Lifecycle
  {
    private readonly IFeature _feature;

    public FeatureLifecycle(IFeature feature) =>
      _feature = feature;
    
    protected override LifecycleException Wrapped(Exception exception) => 
      exception.AsFeatureLifecycleException(_feature);
  }
}