using System;
using System.Collections.Generic;

namespace Common.Extensions
{
  public static class ReadOnlyListExtensions
  {
    public static int? IndexOf<T>(this IReadOnlyList<T> self, Predicate<T> predicate)
    {
      for (var i = 0; i < self.Count; i++)
        if (self[i].Matches(predicate))
          return i;

      return null;
    }

    public static int? IndexOf<T, TU>(this IReadOnlyList<T> self, TU casted) where TU : class
    {
      for (var i = 0; i < self.Count; i++)
        if (self[i] as TU == casted)
          return i;

      return null;
    }
  }
}