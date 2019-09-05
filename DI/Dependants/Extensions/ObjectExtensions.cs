namespace DI.Dependants.Extensions
{
  internal static class ObjectExtensions
  {
    public static Dependant AsDependant(this object instance) =>
      new Dependant(instance);
  }
}