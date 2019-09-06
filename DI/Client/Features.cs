using System;
using System.Collections.Generic;
using Common.Extensions;
using DI.Dependant;
using DI.Dependencies.Extensions;
using DI.Logs;
using DI.Registered;
using DI.Registered.Extensions;
using DI.RegisterExpression;

namespace DI.Client
{
  public class Features
  {
    public event Action<Exception> Failed = delegate { };
    
    private static RegisteredFeatures _registered;
    private static Dependants _dependants;

    private readonly ILog _log;
    public Features(ILog log = null)
    {
      if (_registered != null)
        throw new InvalidOperationException($"Multiple instances of {nameof(Features)} is not allowed.");

      _log = log;
      _registered = new RegisteredFeatures(_log).WhenFailed(Failed.Invoke);
      _dependants = new Dependants(_log, _registered);
    }

    public void Initialize() =>
      _registered.Lifecycle.Start();

    public void Pause() =>
      _registered.Lifecycle.Pause();

    public void Continue() =>
      _registered.Lifecycle.Continue();

    public void Terminate()
    {
      _dependants.Release();
      
      _registered.Lifecycle.Stop();
      _registered = null;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature) where TFeature : Feature => 
       new RegisterFeatureExpression<TFeature>(feature, _registered);

    public static void InjectInto(object instance) =>
      _dependants.Add(instance);

    public static void Clear(object instance) =>
      _dependants.Release(instance);
  }
}