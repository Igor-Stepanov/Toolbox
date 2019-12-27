using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DependencyInjection.API.Dependencies;
using DependencyInjection.Core;
using DependencyInjection.Core.Exceptions;
using DependencyInjection.Core.Lifecycles;
using DependencyInjection.Model.Dictionary;
using DependencyInjection.Model.Lifecycles;
using DependencyInjection.Model.Lifecycles.Extensions;
using static System.Activator;

namespace DependencyInjection.Model
{
  internal class Features : IFeatures
  {
    public ILifecycle Lifecycle { get; }
    
    private bool _initialized;
    
    private readonly OrderedDictionary<Type, Feature> _features = new OrderedDictionary<Type, Feature>();
    private readonly Dictionary<Type, Type> _implementations = new Dictionary<Type, Type>();

    private readonly IExceptions _exceptions;

    public Features(IExceptions exceptions)
    {
      Lifecycle = new Lifecycle(exceptions)
       .InitializeWith(Initialize)
       .UpdateWith(Update)
       .PauseWith(Pause)
       .ContinueWith(Continue)
       .TerminateWith(Terminate);
      
      _exceptions = exceptions;
      _initialized = false;
    }

    void IFeatures.Register(Type feature, Type implementation) =>
      _exceptions.Try(() =>
      {
        if (_initialized)
          throw new InvalidOperationException("Registering new features after initialize is not allowed.");
        
        if (_implementations.TryGetValue(feature, out var registeredImplementation))
          throw new InvalidOperationException(
            $"{registeredImplementation.Name} already registered as {feature.Name} implementation.");

        _implementations.Add(feature, implementation);

        if (_features.Contains(implementation))
          return;

        var instance = (Feature) CreateInstance(implementation);
        instance.Failed += _exceptions.Handle;
        _features.Add(instance);
      });

    

    Feature IFeatures.ImplementationOf(Type feature) =>
      _exceptions.Try(() =>
      {
        if (!_initialized)
          throw new InvalidOperationException("Features not initialized.");
        
        if (!_implementations.TryGetValue(feature, out var implementationType))
          throw new InvalidOperationException($"{feature.Name} not registered.");

        return _features[implementationType];
      });

    void IFeatures.Clear() =>
      _exceptions.Try(() =>
      {
        if (!_initialized)
          throw new InvalidOperationException("Features have not started.");

        _features.Clear();
      });

    private void Initialize()
    {
      foreach (var feature in _features)
        feature.Lifecycle.Initialize();

      _initialized = true;
    }

    private void Update()
    {
      foreach (var feature in _features)
        feature.Lifecycle.Update();
    }

    private async Task Pause()
    {
      foreach (var feature in _features) 
        await feature.Lifecycle.Pause();
    }

    private void Continue()
    {
      foreach (var feature in _features)
        feature.Lifecycle.Continue();
    }

    private async Task Terminate()
    {
      foreach (var feature in _features)
        await feature.Lifecycle.Terminate();
    }
  }
}