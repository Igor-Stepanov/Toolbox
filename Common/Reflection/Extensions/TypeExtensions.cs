using System;

namespace Common.Reflection.Extensions
{
  public static class TypeExtensions
  {
    public static object NewInstance(this Type self) =>
      Activator.CreateInstance(self);

    public static bool Implements<T>(this Type self) => self.Implements(typeof(T));
    public static bool Implements(this Type self, Type other) =>
      other.IsAssignableFrom(self);

    public static bool DerivedFrom<T>(this Type self) => self.DerivedFrom(typeof(T));
    public static bool DerivedFrom(this Type self, Type other)
    {
      if (!other.IsClass)
        throw new InvalidOperationException($"{other.Name} is not a class.");

      if (self == other)
        return false;

      if (!other.IsGenericTypeDefinition)
        return other.IsAssignableFrom(self);

      var isConstructed = self.BaseType?.IsConstructedGenericType ?? false;
      if ((isConstructed ? self.BaseType.GetGenericTypeDefinition() : self.BaseType) == other)
        return true;

      if (!self.IsGenericType)
        return self.BaseType?.DerivedFrom(other) ?? false;

      return self.IsGenericTypeDefinition
        ? self.BaseType.DerivedFrom(other)
        : self.GetGenericTypeDefinition().BaseType.DerivedFrom(other);
    }
  }
}
