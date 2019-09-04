namespace DI.Lifecycle
{
  public interface ILifecycle
  {
    void Initialize();
    void Pause();
    void Continue();
    void Terminate();
  }
}