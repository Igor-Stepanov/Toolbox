using System;
using System.Threading.Tasks;
using DIFeatures.Exceptions;

namespace DIFeatures.Flow
{
  internal interface ILifecycle
  {
    event Action<LifecycleException> Failed;
    
    void Start();
    void Update();
    Task Pause();
    void Continue();
    Task Stop();
  }
}