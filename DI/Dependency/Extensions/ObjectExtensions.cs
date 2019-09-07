using System.Reflection;

namespace FeaturesDI.Dependency.Extensions
{
  internal static class ObjectExtensions
  {
    public static InstanceField Of(this FieldInfo self, object instance) =>
      new InstanceField(instance, self);
  }
}