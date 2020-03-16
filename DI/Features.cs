using System;
using System.Collections.Generic;
using DI.FeatureInstances;

namespace DI
{
  internal class Features
  {
    private readonly Dictionary<Type, Feature> _features;
    private readonly Dictionary<Type, IFeatureInstance> _instances;

    public Features()
    {
      _features = new Dictionary<Type, Feature>();
      _instances = new Dictionary<Type, IFeatureInstance>();
    }

    public void Register(Type type, Type implementationType) => 
      FeatureOf(type)
        .Register(CachedInstanceOf(implementationType));

    public void RegisterUnique(Type type, IFeatureInstance uniqueInstance) => 
      FeatureOf(type)
        .Register(uniqueInstance);

    public T Resolve<T>() =>
      _features[typeof(T)].Resolve<T>();

    public IEnumerable<T> ResolveAll<T>() =>
      _features[typeof(T)].ResolveAll<T>();

    public void Clear()
    {
      _features.Clear();
      _instances.Clear();
    }

    private Feature FeatureOf(Type type)
    {
      if (!_features.TryGetValue(type, out var feature))
        _features.Add(type, feature = new Feature());
      
      return feature;
    }

    private IFeatureInstance CachedInstanceOf(Type type)
    {
      if (!_instances.TryGetValue(type, out var implementation))
        _instances.Add(type, implementation = new FeatureInstance(type));
      
      return implementation;
    }
  }
}