using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Reflection.Extensions;
using static System.Reflection.BindingFlags;

namespace DI.Dependencies.Extensions
{
  internal static class TypeExtensions
  {
    public static TypeDependencies Dependencies(this Type self) =>
      CachedTypeDependencies.Of(self);
    
    public static IEnumerable<FieldInfo> AllFieldsWith<TAttribute>(this Type type) where TAttribute : Attribute
    {
      while (type != null)
      {
        foreach (var field in type.FieldsWith<TAttribute>(Instance | NonPublic))
          yield return field;

        type = type.BaseType;
      }
    }
  }
}