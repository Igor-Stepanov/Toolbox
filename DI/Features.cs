using System;
using DI.Registered;
using DI.Registered.Extensions;
using DI.RegisterExpression;
using DI.Request;
using DI.Tracked;

namespace DI
{
  public class Features
  {
    public event Action<Exception> Failed = delegate { };
    
    private static IRegisteredFeatures _registered;
    private static FeatureReferences _references;

    public Features()
    {
      if (_registered != null)
        throw new InvalidOperationException($"Multiple instances of {nameof(Features)} is not allowed.");

      _registered = new RegisteredFeatures().WhenFailed(Failed.Invoke);
      _references = new FeatureReferences();
    }

    public static FeatureRequest Request() => 
      new FeatureRequest(_registered, _references);

    public void Initialize() =>
      _registered.Lifecycle.Start();

    public void Pause() =>
      _registered.Lifecycle.Pause();

    public void Continue() =>
      _registered.Lifecycle.Continue();

    public void Terminate()
    {
      _references.Release();
      _references = null;
      
      _registered.Lifecycle.Stop();
      _registered = null;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature) where TFeature : Feature => 
       new RegisterFeatureExpression<TFeature>(feature, _registered);
  }
}