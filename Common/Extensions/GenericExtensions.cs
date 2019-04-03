using System;

namespace Common.Extensions
{
  public static class GenericExtensions
  {
    public static T With<T>(this T self, Action<T> action)
    {
      action(self);
      return self;
    }
  }
}