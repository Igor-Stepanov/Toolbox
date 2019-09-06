using DI.Client;
using DI.Lifecycles;
using DI.Registered.Dictionary;

namespace DI.Registered
{
  internal interface IRegisteredFeature
  {
    RegisteredFeatureType Type { get; }
    IFeature Feature { get; }
    ILifecycle Lifecycle { get; }
  }
}