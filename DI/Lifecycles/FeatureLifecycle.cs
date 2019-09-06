using System;
using FeaturesDI.Client;
using FeaturesDI.Exceptions;
using FeaturesDI.Lifecycles.Extensions;

namespace FeaturesDI.Lifecycles
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