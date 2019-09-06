namespace DI.Client
{
  public static class Dependencies
  {
    public static void InjectInto(object dependant) =>
      Features.InjectInto(dependant);

    public static void Clear(object dependant) =>
      Features.Clear(dependant);

    public static void InjectDependencies(this object self) =>
      InjectInto(self);
    
    public static void ClearDependencies(this object self) =>
      Clear(self);
  }
}