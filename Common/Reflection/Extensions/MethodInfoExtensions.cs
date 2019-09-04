using System;
using System.Reflection;

namespace Common.Reflection.Extensions
{
  public static class MethodInfoExtensions
  {
    public static bool Has<TAttribute>(this MethodInfo self) where TAttribute : Attribute =>
      self.GetCustomAttribute<TAttribute>() != null;
  }
}