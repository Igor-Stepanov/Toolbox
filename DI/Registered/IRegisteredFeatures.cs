using System;
using DI.Client;

namespace DI.Registered
{
  internal interface IRegisteredFeatures
  {
    void Add<TFeature>(TFeature feature) where TFeature : Feature;
    void Add(Type abstraction, Type implementation);

    IFeature FeatureOf(Type type);
  }
}