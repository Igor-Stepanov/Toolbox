using System;
using FeaturesDI.Dependant;
using FeaturesDI.Dependencies.Extensions;
using FeaturesDI.Registered;

namespace FeaturesDI.Static
{
  internal class DI
  {
    public static DI Current { get; private set; }

    private readonly IFeatures _features;
    private readonly IDependants _dependants;

    public DI(IFeatures features, IDependants dependants)
    {
      _dependants = dependants;
      _features = features;
    }

    public void InjectInto(object instance)
    {
      var type = _dependants.Types.OneOf(instance.GetType());
      var instance = type.Of(instance);
      _dependants.Add(_dependants.Types.OneOf(instance).InjectedWith(_features));
    }

    public void Release(object instance) => 
      _dependants.Remove(instance.Released());

    public static void Initialize(IFeatures features, IDependants dependants)
    {
      if (Current != null)
        throw new InvalidOperationException($"Multiple instances of {nameof(DI)} is not allowed.");
      
      Current = new DI(features, dependants);
    }
    
    public static void Terminate()
    {
      if (Current == null)
        throw new InvalidOperationException($"Instance of {nameof(DI)} is terminated already.");
      
      Current = null;
    }
  }
}