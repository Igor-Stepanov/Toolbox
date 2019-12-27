using System.Threading.Tasks;

namespace DependencyInjection.Core.Lifecycles
{
  internal interface ILifecycle
  {
    void Initialize();
    void Update();
    Task Pause();
    void Continue();
    Task Terminate();
  }
}