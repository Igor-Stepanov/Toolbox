using System;
using System.Collections;
using System.Collections.Generic;
using FeaturesDI.Client;
using FeaturesDI.Registered.Dictionary;

namespace FeaturesDI.Registered
{
  internal class Features : IFeatures
  {
    private readonly OrderedDictionary<Feature> _features = new OrderedDictionary<Feature>();
    private readonly Dictionary<Type, Type> _implementations = new Dictionary<Type, Type>();

    void IFeatures.Add(Feature feature)
    {
      if (_features.Contains(feature.Type))
        throw new InvalidOperationException($"{feature.Type.Name} registered already.");
      
      _features.Add(feature);
    }

    void IFeatures.Add(Type abstraction, Type implementation)
    {
      if (!_features.Contains(implementation))
        throw new InvalidOperationException($"Please, register {implementation.Name} before {abstraction.Name}.");

      if (_implementations.TryGetValue(abstraction, out var registeredImplementation))
        throw new InvalidOperationException($"{abstraction.Name} implementation already registered as {registeredImplementation.Name}.");
      
      _implementations.Add(abstraction, implementation);
    }
    
    Feature IFeatures.Registered(Type abstractionType)
    {
      if (!_implementations.TryGetValue(abstractionType, out var implementationType))
        throw new InvalidOperationException($"{abstractionType.Name} not registered.");
      
      return _features[implementationType];
    }

    public void Clear() =>
      _features.Clear();

    public IEnumerator<Feature> GetEnumerator() => _features.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}