namespace FeaturesDI.Client.Extensions
{
  public static class ObjectExtensions
  {
    public static void InjectDependencies(this object self) =>
      Static.DI.Current.InjectInto(self);
    
    public static void ReleaseDependencies(this object self) =>
      Static.DI.Current.Release(self);
  }
}