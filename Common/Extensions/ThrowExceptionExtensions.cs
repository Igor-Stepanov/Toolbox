using System;

namespace Common.Extensions
{
  public static class ThrowExceptionExtensions
  {
    public static void Throw(this string self) =>
      throw new Exception(message: self);
    

  }
}