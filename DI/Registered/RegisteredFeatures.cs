using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Common.Collections.OrderedDictionary;
using Common.Extensions;
using DI.Client;
using DI.Lifecycles;
using DI.Lifecycles.Extensions;
using DI.Registered.Dictionary;
using DI.Registered.Extensions;

namespace DI.Registered
{
  internal class RegisteredFeatures : IRegisteredFeatures
  {
    public event Action<Exception> Failed = delegate { };
    public ILifecycle Lifecycle => _lifecycle;

    private readonly RegisteredFeaturesDictionary _registeredFeatures;
    private readonly Lifecycle _lifecycle;

    public RegisteredFeatures()
    {
      _registeredFeatures = new RegisteredFeaturesDictionary();
      _lifecycle = new Lifecycle()
        .StartWith(StartAll)
        .PauseWith(PauseAll)
        .ContinueWith(ContinueAll)
        .StopWith(StopAll);
    }

    void IRegisteredFeatures.Add<TFeature>(TFeature feature) =>
      _registeredFeatures.Add(
        feature
          .Registered()
          .WhenFailed(Failed.Invoke));

    void IRegisteredFeatures.AddImplementationOf<TAbstractFeature>(TAbstractFeature feature) => 
      _registeredFeatures.Add(feature.RegisteredImplementation());
    
    IFeature IRegisteredFeatures.FeatureOf(Type type) =>
      _registeredFeatures[type]
       .Feature;

    private void StartAll() =>
      _registeredFeatures
        .Select(x => x.Lifecycle)
        .ForEach(x => x.Start());

    private void PauseAll() =>
      _registeredFeatures
        .Select(x => x.Lifecycle)
        .Reverse()
        .ForEach(x => x.Pause());

    private void ContinueAll() => 
      _registeredFeatures
        .Select(x => x.Lifecycle)
        .ForEach(x => x.Continue());

    private void StopAll()
    {
      _registeredFeatures
        .Select(x => x.Lifecycle)
        .Reverse()
        .ForEach(x => x.Stop());
      
      _registeredFeatures.Clear();
    }
  }
}