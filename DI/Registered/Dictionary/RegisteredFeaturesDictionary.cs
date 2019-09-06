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
    private readonly Dictionary<Type, Type> _implementations = new Dictionary<Type, Type>();
    
    public void Add(IRegisteredFeature feature)
    {
      if (_dictionary.ContainsKey(feature.Type))
        throw new InvalidOperationException($"Feature {feature.Type.Name} already registered.");
      
      _dictionary.Add(feature.Type, feature);
      _list.Add(feature);
    }
    
    public void Add(Type abstraction, Type implementation)
    {
      if (!_dictionary.ContainsKey(implementation))
        throw new InvalidOperationException($"Please, register {implementation.Name} before {abstraction.Name}.");
      
      _implementations.Add(abstraction, implementation);
    }
    
    public IRegisteredFeature this[Type type] => 
      _implementations.TryGetValue(type, out var implementation) 
        ? _dictionary[implementation]
        : _dictionary[type];

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