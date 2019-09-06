using DI.Lifecycles;
using DI.Lifecycles.Extensions;

namespace DI.Client
{
  public abstract class Feature : IFeature, ILifecycle
  {
    public ILifecycle Lifecycle { get; }

    protected Feature() => 
      Lifecycle = new FeatureLifecycle(this)
          .StartWith(Initialize)
          .PauseWith(Pause)
          .ContinueWith(Continue)
          .StopWith(Terminate);

    protected virtual void Initialize(){}
    protected virtual void Pause(){}
    protected virtual void Continue(){}
    protected virtual void Terminate(){}

    void ILifecycle.Start() => Initialize();
    void ILifecycle.Pause() => Pause();
    void ILifecycle.Continue() => Continue();
    void ILifecycle.Stop() => Terminate();
  }
}