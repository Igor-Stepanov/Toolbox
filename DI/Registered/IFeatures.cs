using System;
using System.Collections.Generic;
using DIFeatures.Public;

namespace DIFeatures.Registered
{
  internal interface IFeatures : IEnumerable<Feature>
  {
    void Register(Type abstraction, Feature implementation);

    Feature ImplementationOf(Type abstraction);
    
    void Clear();
  }
}