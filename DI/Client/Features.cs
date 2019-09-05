using System;
using System.Collections.Generic;
using Common.Extensions;
using DI.Dependants;
using DI.Dependencies.Extensions;
using DI.Registered;
using DI.Registered.Extensions;
using DI.RegisterExpression;

namespace DI.Client
{
  public class Features
  {
    public event Action<Exception> Failed = delegate { };
    
    private static RegisteredFeatures _registered;
    private static HashSet<Dependant> _dependants;

    public Features()
    {
      if (_registered != null)
        throw new InvalidOperationException($"Multiple instances of {nameof(Features)} is not allowed.");

      _registered = new RegisteredFeatures().WhenFailed(Failed.Invoke);
      _dependants = new HashSet<Dependant>();
    }

    public void Initialize() =>
      _registered.Lifecycle.Start();

    public void Pause() =>
      _registered.Lifecycle.Pause();

    public void Continue() =>
      _registered.Lifecycle.Continue();

    public void Terminate()
    {
      _dependants.ForEach(x => x.Dependencies().Clear());
      
      _registered.Lifecycle.Stop();
      _registered = null;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature) where TFeature : Feature => 
       new RegisterFeatureExpression<TFeature>(feature, _registered);

    public static void InjectInto(object instance)
    {
      if (!_dependants.)
      {
      }
    }
    
    public static IFeature RegisteredImplementationOf(Type type) => 
      _registered.FeatureOf(type);
  }
}