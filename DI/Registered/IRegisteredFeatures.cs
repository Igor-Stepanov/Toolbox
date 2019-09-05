using System;
using DI.Lifecycles;

namespace DI.Registered
{
  internal interface IRegisteredFeatures
  {
    event Action<Exception> Failed;
    
    ILifecycle Lifecycle { get; }

    void Add<TFeature>(TFeature another)
      where TFeature : Feature;

    void AddImplementationOf<TAbstractFeature>(TAbstractFeature another)
      where TAbstractFeature : class, IFeature;

    TFeature One<TFeature>()
      where TFeature : class, IFeature;
  }
}