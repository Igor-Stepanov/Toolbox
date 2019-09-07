using System;
using System.Collections;
using System.Collections.Generic;
using FeaturesDI.Client;

namespace FeaturesDI.Registered.Dictionary
{
  internal class OrderedDictionary<TFeature> : IEnumerable<TFeature> where TFeature : Feature
  {
    private readonly List<TFeature> _list = new List<TFeature>();
    private readonly Dictionary<Type, TFeature> _dictionary = new Dictionary<Type, TFeature>();
    
    public void Add(TFeature feature)
    {
      _dictionary.Add(feature.Type, feature);
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

    public IEnumerator<TFeature> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}