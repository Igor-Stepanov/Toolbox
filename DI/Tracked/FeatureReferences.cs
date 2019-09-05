using System.Collections.Generic;
using Common.Extensions;
using DI.Requested;

namespace DI.Tracked
{
  public class FeatureReferences : ITrackedFeatures
  {
    private readonly List<RequestedFeatures> _features = new List<RequestedFeatures>();

    public void Add(RequestedFeatures requested)
    {
      requested.Released += () => _features.Remove(requested);
      _features.Add(requested);
    }

    public void Release() => 
      _features
        .ToArray()
        .ForEach(x => x.Release());
  }
}