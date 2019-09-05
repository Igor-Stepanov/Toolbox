using DI.Dependencies.Extensions;

namespace DI.Client
{
  public static class Dependencies
  {
    public static void InjectInto(object dependant) =>
      dependant.Dependencies().Inject();

    public static void Clear(object dependant) =>
      dependant.Dependencies().Clear();

    public static void InjectDependencies(this object self) =>
      InjectInto(self);
    
    public static void ClearDependencies(this object self) =>
      Clear(self);
  }
}