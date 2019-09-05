using System;
using DI.Lifecycles;

namespace DI.Registered
{
  internal interface IRegisteredFeature
  {
    Type Type { get; }
    ILifecycle Lifecycle { get; }
  }
}