using System;
using DIFeatures.Flow;
using DIFeatures.Flow.Extensions;

namespace DIFeatures.Public
{
  public abstract class Feature : IFeature
  {
    internal Type Type => GetType();
    
    internal ILifecycle Lifecycle =>
      new Lifecycle(this)
       .StartWith(Initialize)
       .UpdateWith(Update)
       .PauseWith(Pause)
       .ContinueWith(Continue)
       .StopWith(Terminate);

    protected virtual void Initialize(){}
    protected virtual void Update(){}
    protected virtual void Pause(){}
    protected virtual void Continue(){}
    protected virtual void Terminate(){}
  }
}