using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI.Dependencies.Extensions
{
  internal static class FileInfoExtensions
  {
    public static TypeDependency AsDependency(this FieldInfo self) =>
      new TypeDependency(self);
    
    public static IEnumerable<TypeDependency> AsDependencies(this IEnumerable<FieldInfo> self) =>
      self.Select(AsDependency);
  }
}