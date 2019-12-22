using System;
using Common.Extensions;
using DIFeatures.Dependant;
using DIFeatures.DependencyInjection;
using DIFeatures.Errors;
using DIFeatures.Flow;
using DIFeatures.Flow.Extensions;
using DIFeatures.Registered;
using DIFeatures.Registered.Extensions;
using DIFeatures.RegisterExpression;
using DIFeatures.Static;

namespace DIFeatures.Public
{
  public class Features : IDependencyInjection
  {
    public event Action<Exception> Error = delegate { };
    
    private readonly IErrors _errors;
    private readonly IFeatures _features;
    private readonly IDependants _dependants;

    private bool _initialized;

    public Features()
    {
      _errors = new SilentNotifications(with: Error.Invoke);
      _features = new Registered.Features(_errors);
      _dependants = new Dependants();

      DI.Initialize(this);
    }
    
    public RegisterFeatureExpression<TFeature> Let<TFeature>() where TFeature : IFeature
    {
      if (_initialized)
        throw new InvalidOperationException("Registering new features after initialize is not allowed.");
      
      return new RegisterFeatureExpression<TFeature>(_features);
    }

    void IDependencyInjection.InjectInto(object instance)
    {
      try
      {
        if (!_initialized)
          throw new InvalidOperationException("Features not initialized.");
          
        _dependants.Inject(instance, _features);
      }
      catch (Exception exception)
      {
        _errors.Handle(exception);
      }
    }

    void IDependencyInjection.Release(object instance)
    {
      try
      {
        if (!_initialized)
          throw new InvalidOperationException("Features not initialized.");

        _dependants.Release(instance);
      }
      catch (Exception exception)
      {
        _errors.Handle(exception);
      }
    }

    public void Initialize()
    {
      _features.ForEach(x => x.Lifecycle.As<ILifecycle>().Start());
      _initialized = true;
    }

    public void Update()
    {
      foreach (var feature in _features)
        feature.Lifecycle
          .As<ILifecycle>()
          .Update();
    }

    public void Pause() => _features
      .Reverse()
      .ForEach(x => x.Lifecycle
        .As<ILifecycle>()
        .Pause()
        ?.Wait(100.Milliseconds()));  

    public void Continue() => _features
      .ForEach(x => x.Lifecycle
        .As<ILifecycle>()
        .Continue());

    public void Terminate()
    {
      _features
        .Reverse()
        .ForEach(x => x.Lifecycle
          .As<ILifecycle>()
          .Stop()
          ?.Wait(100.Milliseconds()));
      
      _features.Clear();
      _dependants.ReleaseAll();
      DI.Terminate();
    }
  }
}