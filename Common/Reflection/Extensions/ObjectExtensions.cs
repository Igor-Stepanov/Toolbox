using System;

namespace Common.Reflection.Extensions
{
  public static class ObjectExtensions
  {
    public static Type Type<T>(this T self) =>
      self.GetType();
  }
}