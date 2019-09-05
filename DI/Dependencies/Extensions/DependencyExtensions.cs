using System.Collections.Generic;
using System.Linq;

namespace DI.Dependencies.Extensions
{
  internal static class DependencyExtensions
  {
    public static IEnumerable<InstanceDependency> Of(this IEnumerable<TypeDependency> self, object instance) =>
      self.Select(x => x.Of(instance));
  }
}