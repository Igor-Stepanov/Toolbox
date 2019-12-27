using System;
using DependencyInjection.Core;

namespace DependencyInjection.Model
{
  internal static class DI
  {
    private static IDependencyInjection _container;
    internal static IDependencyInjection Container 
    {
      get
      {
        if (_container == null)
          throw new InvalidOperationException("DI no initialized.");

        return _container;
      }
    }

    internal static void Initialize(IDependencyInjection container)
    {
      if (_container != null)
        throw new InvalidOperationException("Multiple instances of DI is not allowed.");

      _container = container;
    }

    internal static void Clear()
    {
      if (_container == null)
        throw new InvalidOperationException("Instance of DI is terminated already.");

      _container = null;
    }
  }
}