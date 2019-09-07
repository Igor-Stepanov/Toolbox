using System;
using FeaturesDI.Client;
using FeaturesDI.Exceptions;
using FeaturesDI.Lifecycles.Extensions;
using FeaturesDI.Lifecycles.SafeActions;
using FeaturesDI.Lifecycles.SafeActions.Extensions;

namespace FeaturesDI.Lifecycles
{
  public class Lifecycle : ILifecycle
  {
    public event Action<LifecycleException> Failed = delegate { };
    
    public event Action Start;
    public event Action Pause = delegate {  };
    public event Action Continue = delegate {  };
    public event Action Stop = delegate {  };

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

    private void Safe(Action action) => new Safe(action)
       .WhenFailed(Notify)
       .Invoke();
    
    private void Notify(Exception exception) =>
      Failed.Invoke(exception.AsLifecycleException(_feature));
  }
}