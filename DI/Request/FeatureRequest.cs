using System;
using DI.Registered;
using DI.Requested;
using DI.Requested.Extensions;
using DI.Tracked;

namespace DI.Request
{  
  public class FeatureRequest
  {
    private bool _closed;
    private RequestedFeatures _requested;

    private readonly IRegisteredFeatures _registered;
    private readonly ITrackedFeatures _tracked;

    internal FeatureRequest(IRegisteredFeatures registeredFeatures, ITrackedFeatures trackedFeatures) =>
      (_registered, _tracked) =
      (registeredFeatures, trackedFeatures);

    public FeatureRequest Obtain<TReference>(Feature<TReference> featureReference) where TReference : class, IFeature
    {
      if (_closed)
        throw new ObjectDisposedException($"FeatureRequest closed. Make sure not to cache it.");

      var feature = _registered.One<TReference>();
      if (feature == null)
        throw new InvalidOperationException($"Feature [ {typeof(TReference).Name} ] not registered.");

      featureReference() = feature;

      if (_requested == null)
        _requested = new RequestedFeatures();
      
      _requested.Add(featureReference.Requested());
      return this;
    }

    public RequestedFeatures Close()
    {
      _closed = true;

      if (_requested != null)
        _tracked.Add(_requested);
      
      return _requested;
    }
  }
}