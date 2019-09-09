using System;
using Common.Extensions;
using DIFeatures.Dependant;
using DIFeatures.Errors;
using DIFeatures.Registered;
using DIFeatures.Registered.Extensions;
using DIFeatures.RegisterExpression;
using DIFeatures.Static;

namespace DIFeatures.Public
{
  public class Features
  {
    public event Action<Exception> Error = delegate { };
    
    private readonly IErrors _errors;
    private readonly IFeatures _features;
    private readonly IDependants _dependants;

    public Features()
    {
      _errors = new SilentNotifications(with: Error.Invoke);
      _features = new Registered.Features(_errors);
      _dependants = new Dependants();

      DI.Initialize(_errors, _features, _dependants); // TODO: Forbid injections before Initialize
    }
    
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature) // TODO: If forbidden throw exception
      where TFeature : Feature =>
      new RegisterFeatureExpression<TFeature>(_errors, feature, _features);

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