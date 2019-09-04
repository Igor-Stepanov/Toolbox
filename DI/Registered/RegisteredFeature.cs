using System;
using Common.Collections.OrderedDictionary;
using DI.Lifecycle;

namespace DI.Registered
{
  internal class Registered<TFeature> : RegisteredImplementationOf<TFeature>, ILifecycle where TFeature : Feature
  {
    public event Action<Exception> Failed;
    
    public Registered(TFeature feature) : base(feature)
    {
    }

    void ILifecycle.Initialize() =>
      Safe(x => x.Initialize());

    void ILifecycle.Pause() =>
      Safe(x => x.Pause());

    void ILifecycle.Continue() =>
      Safe(x => x.Continue());

    void ILifecycle.Terminate() =>
      Safe(x => x.Terminate());

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