using System;
using DependencyInjection.API.Dependencies;
using DependencyInjection.Core.Lifecycles;

namespace DependencyInjection.Core
{
  internal interface IFeatures
  {
    ILifecycle Lifecycle { get; }

    void Register(Type feature, Type implementation);
    Feature ImplementationOf(Type feature);

    void Clear();
  }
}