using System;
using System.Collections.Generic;
using DIFeatures.Errors;
using DIFeatures.Public;
using DIFeatures.Registered.Dictionary;
using static System.Activator;

namespace DIFeatures.Registered
{
  internal class Features : IFeatures
  {
    private readonly OrderedDictionary<Type, Feature> _features = new OrderedDictionary<Type, Feature>();
    private readonly Dictionary<Type, Type> _implementations = new Dictionary<Type, Type>();

    private readonly IErrors _errors;

    public Features(IErrors errors) => 
      _errors = errors;

    void IFeatures.Register(Type featureType, Type implementationType)
    {
      if (_implementations.TryGetValue(featureType, out var registeredImplementation))
        throw new InvalidOperationException($"{registeredImplementation.Name} already registered as {featureType.Name} implementation.");

      _implementations.Add(featureType, implementationType);

      if (_features.Contains(implementationType))
        return;
      
      var implementation = (Feature) CreateInstance(implementationType);
      implementation.Lifecycle.Failed += _errors.Handle;
      _features.Add(implementation);
    }
    
    Feature IFeatures.ImplementationOf(Type feature)
    {
      if (!_implementations.TryGetValue(feature, out var implementationType))
        throw new InvalidOperationException($"{feature.Name} not registered.");
      
      return _features[implementationType];
    }

    public void Clear() =>
      _features.Clear();
    
    public List<Feature>.Enumerator GetEnumerator() => _features.GetEnumerator();
  }
}