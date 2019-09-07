using FeaturesDI.Static;

namespace FeaturesDI.Client.Extensions
{
  public static class ObjectExtensions
  {
    public static void InjectDependencies(this object self) =>
      DI.Features.InjectInto(self);
    
    public static void ReleaseDependencies(this object self) =>
      DI.Features.Release(self);
  }
}