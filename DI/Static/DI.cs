using System;
using DIFeatures.DependencyInjection;

namespace DIFeatures.Static
{
  internal static class DI
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