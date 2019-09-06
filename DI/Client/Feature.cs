using DI.Lifecycles;
using DI.Lifecycles.Extensions;

namespace DI.Client
{
  public abstract class Feature : IFeature
  {
    public ILifecycle Lifecycle => new FeatureLifecycle(this)
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