using System;
using DIFeatures.Dependant;
using DIFeatures.DependencyInjection;
using DIFeatures.Errors;
using DIFeatures.Registered;
using DIFeatures.ThreadSafe;
using Features = DIFeatures.Public.Features;

namespace DIFeatures.Static
{
  internal class DI
  {
    public static IDependencyInjection Features { get; private set; }
    
    internal static void Initialize(IDependencyInjection features)
    {
      if (Features != null)
        throw new InvalidOperationException("Multiple instances of DI is not allowed.");

      Features = features;
    }

    internal static void Terminate()
    {
      if (Features == null)
        throw new InvalidOperationException("Instance of DI is terminated already.");

      Features = null;
    }
  }
}