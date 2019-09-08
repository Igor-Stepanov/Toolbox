using System;
using DIFeatures.Exceptions;

namespace DIFeatures.Flow
{
  public interface ILifecycle
  {
    event Action<LifecycleException> Failed;
    
    void Start();
    void Update();
    void Pause();
    void Continue();
    void Stop();
  }
}