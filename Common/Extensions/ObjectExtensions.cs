using System;

namespace Common.Extensions
{
  public static class ObjectExtensions
  {
    public static Type Type(this object self) =>
      self.GetType();
    
    public static T As<T>(this object self) where T : class => 
      self as T;

    public static void As<T>(this object self, Action<T> action) where T : class
    {
      if (self is T casted)
          action?.Invoke(casted);
    }
  }
}
