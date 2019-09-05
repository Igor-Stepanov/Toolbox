using System;
using DI.Client;

namespace DI.Registered
{
  internal interface IRegisteredFeatures
  {
    void Add<TFeature>(TFeature another)
      where TFeature : Feature;

    void AddImplementationOf<TAbstractFeature>(TAbstractFeature another)
      where TAbstractFeature : class, IFeature;

    IFeature FeatureOf(Type type);
  }
}