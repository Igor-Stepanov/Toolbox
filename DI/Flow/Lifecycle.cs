using System;
using System.Threading.Tasks;
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
    public event Func<Task> Pause;
    public event Action Continue;
    public event Func<Task> Stop;

    private readonly IFeature _feature;

    public Lifecycle(IFeature feature) =>
      _feature = feature;
    
    void ILifecycle.Start() =>
      Safe(Start);

    Task ILifecycle.Pause() => 
      Safe(Pause);

    void ILifecycle.Continue() => 
      Safe(Continue);

    Task ILifecycle.Stop() => 
      Safe(Stop);

    void ILifecycle.Update() =>
      Safe(Update);

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
    
    private Task Safe(Func<Task> action)
    {
      try
      {
        return action?.Invoke();
      }
      catch (Exception exception)
      {
        Failed?.Invoke(exception.AsLifecycleException(_feature));
        return null;
      }
    }
  }
}