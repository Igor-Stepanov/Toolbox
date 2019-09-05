using System;
using DI.Client;
using DI.Lifecycles;

namespace DI.Registered
{
  internal interface IRegisteredFeature
  {
    Type Type { get; }

    IFeature Feature { get; }
    ILifecycle Lifecycle { get; }
  }
}