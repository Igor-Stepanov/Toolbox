using System.Reflection;

namespace FeaturesDI.Dependencies.Extensions
{
  internal static class FileInfoExtensions
  {
    public static Field AsField(this FieldInfo self) =>
      new Field(self);
  }
}