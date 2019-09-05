using System;
using System.Reflection;

namespace Common.Reflection.Extensions
{
  public static class FieldInfoExtensions
  {
    public static bool Has<TAttribute>(this FieldInfo self) where TAttribute : Attribute =>
      self.GetCustomAttribute<TAttribute>() != null;
  }
}