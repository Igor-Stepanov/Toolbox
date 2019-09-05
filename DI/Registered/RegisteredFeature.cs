using System;
using Common.Collections.OrderedDictionary;
using DI.Client;
using DI.Lifecycles;

namespace DI.Registered
{
  internal class Registered<TFeature> : RegisteredImplementationOf<TFeature>, ILifecycle where TFeature : Feature
  {
    public event Action<Exception> Failed;
    
    public Registered(TFeature feature) : base(feature)
    {
    }

    void ILifecycle.Start() =>
      Safe(x => x.Start());

    void ILifecycle.Pause() =>
      Safe(x => x.Pause());

    void ILifecycle.Continue() =>
      Safe(x => x.Continue());

    void ILifecycle.Stop() =>
      Safe(x => x.Stop());

    private void Safe(Action<ILifecycle> action)
    {
      try
      {
        action(Feature);
      }
      catch (Exception exception)
      {
        Failed?.Invoke(exception);
      }
    }
  }
}