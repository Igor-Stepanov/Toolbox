using DependencyInjection.Core;
using DependencyInjection.Model;

namespace DependencyInjection.API.Injection
{
  public static class InjectExtensions
  {
    public static void InjectDependencies(this object self) =>
      DI.Container.Inject(self);

    public static void ReleaseDependencies(this object self) =>
      DI.Container.Release(self);
  }
}