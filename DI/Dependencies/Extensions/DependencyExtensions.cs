using System.Collections.Generic;
using System.Linq;

namespace FeaturesDI.Dependencies.Extensions
{
  internal static class DependencyExtensions
  {
    public static IEnumerable<InstanceField> Of(this IEnumerable<Field> self, object instance) =>
      self.Select(x => x.Of(instance));
  }
}