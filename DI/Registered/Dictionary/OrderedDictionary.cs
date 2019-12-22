using System;
using System.Collections.Generic;
using DIFeatures.Public;

namespace DIFeatures.Registered.Dictionary
{
  internal class OrderedDictionary<TType, TFeature>
    where TFeature : Feature
    where TType : Type
  {
    private readonly List<TFeature> _list = new List<TFeature>();
    private readonly Dictionary<Type, TFeature> _dictionary = new Dictionary<Type, TFeature>();
    
    public void Add(TFeature feature)
    {
      _dictionary.Add(typeof(TFeature), feature);
      _list.Add(feature);
    }

    public bool Contains(Type featureType) =>
      _dictionary.ContainsKey(featureType);
    
    public TFeature this[Type featureType] => 
      _dictionary[featureType];

    public void Clear()
    {
      _list.Clear();
      _dictionary.Clear();
    }
    
    public List<TFeature>.Enumerator GetEnumerator() =>
      _list.GetEnumerator();
  }
}