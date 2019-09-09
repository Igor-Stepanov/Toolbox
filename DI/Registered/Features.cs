using System;
using System.Collections.Generic;
using DIFeatures.Errors;
using DIFeatures.Public;
using DIFeatures.Registered.Dictionary;

namespace DIFeatures.Registered
{
  internal class Features : IFeatures
  {
    private readonly OrderedDictionary<Feature> _features = new OrderedDictionary<Feature>();
    private readonly Dictionary<Type, Type> _implementations = new Dictionary<Type, Type>();

    private readonly IErrors _errors;

    public Features(IErrors errors) => 
      _errors = errors;

    void IFeatures.Register(Type abstraction, Feature implementation)
    {
      if (_implementations.TryGetValue(abstraction, out var registeredImplementation))
        throw new InvalidOperationException($"{registeredImplementation.Name} already registered as {abstraction.Name} implementation.");

      if (!_features.Contains(implementation.Type))
      {
        implementation.Lifecycle.Failed += _errors.Handle;
        _features.Add(implementation);
      }

      _implementations.Add(abstraction, implementation.Type);
    }
    
    Feature IFeatures.ImplementationOf(Type abstractionType)
    {
      if (!_implementations.TryGetValue(abstractionType, out var implementationType))
        throw new InvalidOperationException($"{abstractionType.Name} not registered.");
      
      return _features[implementationType];
    }

    public void Clear() =>
      _features.Clear();
    
    public List<Feature>.Enumerator GetEnumerator() => _features.GetEnumerator();
  }
}