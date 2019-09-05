using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Collections.OrderedDictionary;

namespace DI.Registered.Dictionary
{
  internal class RegisteredFeaturesDictionary : IEnumerable<IRegisteredFeature>
  {
    private readonly List<IRegisteredFeature> _list = new List<IRegisteredFeature>();
    private readonly Dictionary<Type, IRegisteredFeature> _dictionary = new Dictionary<Type, IRegisteredFeature>();
    
    public void Add(IRegisteredFeature feature)
    {
      if (_dictionary.ContainsKey(feature.Type))
        throw new InvalidOperationException($"Feature {feature.Type.Name} already registered.");
      
      _dictionary.Add(feature.Type, feature);
      _list.Add(feature);
    }

    public IEnumerable<T> Implementing<T>() => _list
      .Where(x => x is T)
      .Cast<T>();

    public IRegisteredFeature this[Type type] =>
      _dictionary[type];
    
    public void Clear()
    {
      _list.Clear();
      _dictionary.Clear();
    }

    public IEnumerator<IRegisteredFeature> GetEnumerator() => 
      _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
      _list.GetEnumerator();
  }
}