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
    public event Action<Exception> Failed;
    
    private static RegisteredFeatures _registered;
    private static TrackedFeatures _tracked;

    public Features()
    {
      if (_registered != null)
        throw new InvalidOperationException($"Multiple instances of {nameof(Features)} is not allowed.");

      _registered = new RegisteredFeatures().WhenFailed(NotifyFailed);
      _tracked = new TrackedFeatures();
    }

    public static FeatureRequest Request() => 
      new FeatureRequest(_registered, _tracked);

    public void Initialize() =>
      _registered.Initialize();

    public void Pause() =>
      _registered.Pause();

    public void Continue() =>
      _registered.Continue();

    public void Terminate()
    {
      _tracked.Terminate();
      _tracked = null;
      
      _registered.Terminate();
      _registered = null;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature) where TFeature : Feature => 
       new RegisterFeatureExpression<TFeature>(feature, _registered);

    private void NotifyFailed(Exception exception) =>
      Failed?.Invoke(exception);
  }
}