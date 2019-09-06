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
    private readonly Dictionary<RegisteredFeatureType, IRegisteredFeature> _dictionary 
      = new Dictionary<RegisteredFeatureType, IRegisteredFeature>();
    private readonly Dictionary<Type, Type> _abstractions = new Dictionary<Type, Type>();
    
    public void Add(IRegisteredFeature feature)
    {
      if (_dictionary.ContainsKey(feature.Type))
        throw new InvalidOperationException($"Feature {feature.Type.Name} already registered.");
      
      _dictionary.Add(feature.Type, feature);
      _list.Add(feature);
    }
    
    public void Add(Type feature, Type abstraction)
    {
      if (!_dictionary.TryGetValue(feature, out var value))
        throw new InvalidOperationException($"Please, register {feature.Name} before {abstraction.Name} abstraction.");
      
      value.Type
      _abstractions.Add(abstraction, feature);
    }
    
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