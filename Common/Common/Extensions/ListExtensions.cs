using System;
using System.Collections.Generic;

namespace Common.Extensions
{
  public static class ListExtensions
  {
    public static void ForEach<T>(this IList<T> self, Action<T> action)
    {
      // ReSharper disable once ForCanBeConvertedToForeach
      for (var i = 0; i < self.Count; ++i)
        action?.Invoke(self[i]);
    }
  }
}
