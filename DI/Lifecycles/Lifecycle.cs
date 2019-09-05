using System;
using DI.Exceptions;
using DI.Lifecycles.Extensions;

namespace DI.Lifecycles
{
  public class Lifecycle : ILifecycle
  {
    public event Action<LifecycleException> Failed = delegate { };
    
    public event Action Start = delegate {  };
    public event Action Pause = delegate {  };
    public event Action Continue = delegate {  };
    public event Action Stop = delegate {  };

    void ILifecycle.Start() =>
      Safe(Start);

    void ILifecycle.Pause() => 
      Safe(Pause);

    void ILifecycle.Continue() => 
      Safe(Continue);

    void ILifecycle.Stop() => 
      Safe(Stop);

    private void Safe(Action action)
    {
      try
      {
        action();
      }
      catch (Exception exception)
      {
        Failed.Invoke(Wrapped(exception));
      }
    }
    
    protected virtual LifecycleException Wrapped(Exception exception) =>
      exception.AsLifecycleException();
  }
}