using System;
using System.Collections.Generic;
using System.Linq;
using DIFeatures.Public;

namespace DIFeatures.Registered.Extensions
{
  internal static class FeaturesExtensions
  {
    public static void ForEach(this IFeatures self, Action<Feature> action)
    {
      foreach (var feature in self)
        action?.Invoke(feature);
    }

    public static IEnumerable<Feature> Reverse(this IFeatures self) =>
      self.AsEnumerable()
        .Reverse();


    private static IEnumerable<Feature> AsEnumerable(this IFeatures self)
    {
      foreach (var feature in self)
        yield return feature;
    }
      
  }
}