using System;
using System.Reflection;

namespace Common.Reflection.Extensions
{
  public static class AssemblyExtensions
  {
    public static Type[] Types(this Assembly self) =>
      self.GetTypes();
  }
}