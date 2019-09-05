namespace DI.Dependencies.Extensions
{
  internal static class ObjectExtensions
  {
    public static InstanceDependencies Dependencies(this object self) => self
     .GetType()
     .Dependencies()
     .Of(self);
  }
}