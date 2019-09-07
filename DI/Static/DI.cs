using System;
using FeaturesDI.Dependant;
using FeaturesDI.Registered;

namespace FeaturesDI.Static
{
  internal class DI
  {
    public static DI Features { get; private set; }

    private readonly IFeatures _features;
    private readonly IDependants _dependants;

    public DI(IFeatures features, IDependants dependants)
    {
      _dependants = dependants;
      _features = features;
    }

    public void InjectInto(object instance) =>
      _dependants.Inject(instance, _features);

    public void Release(object instance) => 
      _dependants.Release(instance);

    public static void Initialize(IFeatures features, IDependants dependants)
    {
      if (Features != null)
        throw new InvalidOperationException($"Multiple instances of {nameof(DI)} is not allowed.");
      
      Features = new DI(features, dependants);
    }
    
    public static void Terminate()
    {
      if (Features == null)
        throw new InvalidOperationException($"Instance of {nameof(DI)} is terminated already.");
      
      Features = null;
    }
  }
}