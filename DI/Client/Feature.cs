using System;
using FeaturesDI.Lifecycles;
using FeaturesDI.Lifecycles.Extensions;

namespace FeaturesDI.Client
{
  public abstract class Feature : IFeature
  {
    internal Type Type => GetType();
    
    public ILifecycle Lifecycle => new Lifecycle(this)
       .StartWith(Initialize)
       .PauseWith(Pause)
       .ContinueWith(Continue)
       .StopWith(Terminate);

    protected virtual void Initialize(){}
    protected virtual void Pause(){}
    protected virtual void Continue(){}
    protected virtual void Terminate(){}
  }
}