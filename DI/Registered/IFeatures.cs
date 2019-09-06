using System;
using System.Collections.Generic;
using FeaturesDI.Client;

namespace FeaturesDI.Registered
{
  internal interface IFeatures : IEnumerable<Feature>
  {
    void Add(Feature feature);
    void Add(Type abstraction, Type implementation);

    Feature Registered(Type abstraction);
    
    void Clear();
  }
}