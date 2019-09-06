namespace FeaturesDI.Lifecycles
{
  public interface ILifecycle
  {
    void Start();
    void Pause();
    void Continue();
    void Stop();
  }
}