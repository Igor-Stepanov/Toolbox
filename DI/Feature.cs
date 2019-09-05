using DI.Lifecycles;

namespace DI
{
  public abstract class Feature : IFeature, ILifecycle
  {
    protected virtual void Initialize(){}
    protected virtual void Pause(){}
    protected virtual void Continue(){}
    protected virtual void Terminate(){}

    void ILifecycle.Start() => Initialize();
    void ILifecycle.Pause()      => Pause();
    void ILifecycle.Continue()   => Continue();
    void ILifecycle.Stop()  => Terminate();
  }
}