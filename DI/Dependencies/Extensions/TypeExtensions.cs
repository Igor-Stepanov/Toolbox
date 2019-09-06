using System;
using System.Collections.Generic;
using Common.Reflection.Extensions;
using static System.Reflection.BindingFlags;

namespace FeaturesDI.Dependencies.Extensions
{
  internal static class TypeExtensions
  {
    public static TypeDependencies Dependencies(this Type self) =>
      CachedTypeDependencies.Of(self);
    
    public static IEnumerable<Field> FieldsWith<TAttribute>(this Type type) where TAttribute : Attribute
    {
      while (type != null)
      {
        foreach (var fieldInfo in type.FieldsWith<TAttribute>(Instance | NonPublic))
          yield return fieldInfo.AsField();

        type = type.BaseType;
      }
    }
  }
}