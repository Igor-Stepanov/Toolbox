using System;
using System.Collections.Generic;

namespace Common.Extensions
{
  public static class GenericExtensions
  {
    public static T With<T>(this T self, Action<T> action)
    {
      action(self);
      return self;
    }
    
    public static bool EqualsAnyOf<T>(this T self, IEnumerable<T> other)
    {
      if (self == null || other == null)
        return false;
      
      if (other is IList<T> list)
        for (var i = 0; i < list.Count; i++)
          if (list[i].Equals(self))
            return true;

      foreach (var item in other)
        if (item.Equals(self))
          return true;

      return false;
    }
  }
}