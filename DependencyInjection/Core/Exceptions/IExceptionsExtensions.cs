using System;

namespace DependencyInjection.Core.Exceptions
{
  // ReSharper disable once InconsistentNaming
  internal static class IExceptionsExtensions
  {
    public static void Try(this IExceptions self, Action action)
    {
      try
      {
        action();
      }
      catch (Exception exception)
      {
        self.Handle(exception);
      }
    }

    public static T Try<T>(this IExceptions self, Func<T> func, T orReturn = default)
    {
      try
      {
        return func();
      }
      catch (Exception exception)
      {
        self.Handle(exception);
        return orReturn;
      }
    }
  }
}