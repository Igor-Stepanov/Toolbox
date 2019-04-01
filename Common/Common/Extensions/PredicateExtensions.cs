using System;

namespace Common.Extensions
{
  public static class PredicateExtensions
  {
    public static bool Matches<T>(this T self, Predicate<T> match)
    {
      return match(self);
    }

    public static bool NullOrMatches<T>(this Predicate<T> self, T target)
    {
      switch (self)
      {
        case null:
          return true;

        default:
          return target.Matches(self);
      }
    }
  }
}
