using System;
using Common.Extensions;
using DIFeatures.Dependant;
using DIFeatures.DependencyInjection;
using DIFeatures.Errors;
using DIFeatures.Registered;
using DIFeatures.Registered.Extensions;
using DIFeatures.RegisterExpression;
using DIFeatures.Static;
using DIFeatures.ThreadSafe;

namespace DIFeatures.Public
{
  public class Features : IDependencyInjection
  {
    public event Action<Exception> Error = delegate { };
    
    private readonly IErrors _errors;
    private readonly IFeatures _features;
    private readonly IDependants _dependants;
    private readonly ThreadSafeSection _threadSafe;

    public Features()
    {
      _errors = new SilentNotifications(with: Error.Invoke);
      _features = new Registered.Features(_errors);
      _dependants = new Dependants();
      _threadSafe = new ThreadSafeSection();

      DI.Initialize(this); // TODO: Forbid injections before Initialize
    }
    
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature) // TODO: If forbidden throw exception
      where TFeature : Feature =>
      new RegisterFeatureExpression<TFeature>(_errors, feature, _features);
    
    void IDependencyInjection.InjectInto(object instance)
    {
      try
      {
        using (_threadSafe.Section)
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
        using (_threadSafe.Section)
          _dependants.Release(instance);
      }
      catch (Exception exception)
      {
        _errors.Handle(exception);
      }
    }

    public void Initialize() => _features // TODO: After this forbid register
      .ForEach(x => x.Lifecycle.Start());
    
    public void Update()
    {
      foreach (var feature in _features)
        feature.Lifecycle.Update();
    }

    public void Pause() => _features
      .Reverse()
      .ForEach(x => x.Lifecycle.Pause());

    public void Continue() => _features
      .ForEach(x => x.Lifecycle.Continue());

    public void Terminate()
    {
      _features
       .Reverse()
       .ForEach(x => x.Lifecycle.Stop());
      
      _features.Clear();
      _dependants.ReleaseAll();
      
      DI.Terminate();
    }
  }
}