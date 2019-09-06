using System;
using DI.Client;
using DI.Lifecycles;
using DI.Lifecycles.Extensions;

namespace DI.Registered
{
  internal class Registered<TFeature> : IRegisteredFeature where TFeature : Feature
  {
    public event Action<Exception> Failed;

    public Type Type => typeof(TFeature);
    public IFeature Feature => _feature;
    public ILifecycle Lifecycle { get; }

    private readonly TFeature _feature;

    public Registered(TFeature feature)
    {
      _feature = feature;
      Lifecycle = new Lifecycle()
        .StartWith(Safe(_feature.Lifecycle.Start))
        .PauseWith(Safe(_feature.Lifecycle.Pause))
        .ContinueWith(Safe(_feature.Lifecycle.Continue))
        .StopWith(Safe(_feature.Lifecycle.Stop));
    }

    private Action Safe(Action action) => () =>
    {
      try
      {
        action();
      }
      catch (Exception exception)
      {
        Failed?.Invoke(exception);
      }
    };
  }
}