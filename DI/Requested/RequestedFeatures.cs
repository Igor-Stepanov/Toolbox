using System;
using System.Collections.Generic;
using System.Linq;

namespace DI.Requested
{
  public class RequestedFeatures
  {
    public event Action Released;
    
    private readonly List<IRequestedFeature> _features = new List<IRequestedFeature>();

    public void Add(IRequestedFeature feature) =>
      _features.Add(feature);

    public bool Any() =>
      _features.Any();

    public void Release()
    {
      _features.ForEach(x => x.ReleaseReference());
      Released?.Invoke();
    }
  }
}