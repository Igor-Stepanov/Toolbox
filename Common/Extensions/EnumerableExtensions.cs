using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
      if (self != null)
        foreach (var item in self)
          action?.Invoke(item);

      return self;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
      switch (enumerable)
      {
        case null:
          return true;

        case ICollection<T> collection:
          return collection.Count == 0;

        default:
          return !enumerable.Any();
      }
    }
  }
}
