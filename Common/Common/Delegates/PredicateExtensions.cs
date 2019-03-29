using System;

namespace Common.Delegates
{
  public static class PredicateExtensions
  {
    public static bool Matches<T>(this T self, Predicate<T> match)
    {
      return match(self);
    }
  }
}
