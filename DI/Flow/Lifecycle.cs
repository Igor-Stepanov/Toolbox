using System;
using DIFeatures.Exceptions;
using DIFeatures.Flow.Extensions;
using DIFeatures.Public;

namespace DIFeatures.Flow
{
  public class Lifecycle : ILifecycle
  {
    public event Action<LifecycleException> Failed;
    
    public event Action Start;
    public event Action Update;
    public event Action Pause;
    public event Action Continue;
    public event Action Stop;

    private readonly IFeature _feature;

    public Lifecycle(IFeature feature) =>
      _feature = feature;
    
    void ILifecycle.Start() =>
      Safe(Start);

    void ILifecycle.Pause() => 
      Safe(Pause);

    void ILifecycle.Continue() => 
      Safe(Continue);

    void ILifecycle.Stop() => 
      Safe(Stop);

    void ILifecycle.Update()
    {
      try
      {
        Update?.Invoke();
      }
      catch (Exception exception)
      {
        Failed?.Invoke(exception.AsLifecycleException(_feature));
      }
    }

    private void Safe(Action action)
    {
      try
      {
        action?.Invoke();
      }
      catch (Exception exception)
      {
        Failed?.Invoke(exception.AsLifecycleException(_feature));
      }
    }
  }
}