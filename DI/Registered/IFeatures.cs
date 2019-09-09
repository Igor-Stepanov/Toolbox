using System;
using System.Collections.Generic;
using DIFeatures.Public;

namespace DIFeatures.Registered
{
  internal interface IFeatures
  {
    void Register(Type abstraction, Feature implementation);

    Feature ImplementationOf(Type abstraction);

    List<Feature>.Enumerator GetEnumerator();
    
    void Clear();
  }
}