using System;
using System.Linq;
using Common.Collections.OrderedDictionary;
using Common.Extensions;
using DI.Lifecycle;
using DI.Registered.Dictionary;
using DI.Registered.Extensions;

namespace DI.Registered
{
  internal class RegisteredFeatures : IRegisteredFeatures, ILifecycle
  {
    public event Action<Exception> Failed = delegate { };
    
    private readonly RegisteredFeaturesDictionary _registeredFeatures = new RegisteredFeaturesDictionary();

    void IRegisteredFeatures.Add<TFeature>(TFeature feature) => 
      _registeredFeatures.Add(feature
        .Registered()
        .WhenFailed(Failed.Invoke));

    void IRegisteredFeatures.AddImplementationOf<TAbstractFeature>(TAbstractFeature feature) => 
      _registeredFeatures.Add(feature.RegisteredImplementation());

    TFeature IRegisteredFeatures.One<TFeature>() =>
      _registeredFeatures[typeof(TFeature)]
        .As<TFeature>();

    void ILifecycle.Start() => 
      _registeredFeatures
        .Implementing<ILifecycle>()
        .ForEach(x => x.Start());

    void ILifecycle.Pause() =>
      _registeredFeatures
        .Implementing<ILifecycle>()
        .Reverse()
        .ForEach(x => x.Pause());

    void ILifecycle.Continue() => 
      _registeredFeatures
        .Implementing<ILifecycle>()
        .ForEach(x => x.Continue());

    void ILifecycle.Stop()
    {
      _registeredFeatures
        .Implementing<ILifecycle>()
        .Reverse()
        .ForEach(x => x.Stop());
      
      _registeredFeatures.Clear();
    }
  }
}