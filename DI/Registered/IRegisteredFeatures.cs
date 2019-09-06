using System;
using DI.Client;

namespace DI.Registered
{
  internal interface IRegisteredFeatures
  {
    void Add<TFeature>(TFeature feature) where TFeature : Feature;

    void AddImplementationOf(Type abstraction, IFeature implementation);

    IFeature FeatureOf(Type type);
  }
}