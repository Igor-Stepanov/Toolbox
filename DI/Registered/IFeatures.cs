using System;
using System.Collections.Generic;
using DIFeatures.Public;

namespace DIFeatures.Registered
{
  internal interface IFeatures
  {
    Feature ImplementationOf(Type feature);

    void Clear();

    void Register(Type featureType, Type implementationType);

    List<Feature>.Enumerator GetEnumerator();
  }
}