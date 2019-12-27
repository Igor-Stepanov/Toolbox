using System.Reflection;

namespace DependencyInjection.Model.Dependencies.Extensions
{
  internal static class ObjectExtensions
  {
    public static InstanceField Of(this FieldInfo self, object instance) =>
      new InstanceField(instance, self);
  }
}