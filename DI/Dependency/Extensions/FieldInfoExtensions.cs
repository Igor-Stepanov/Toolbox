using System;
using System.Reflection;

namespace DIFeatures.Dependency.Extensions
{
  public static class FieldInfoExtensions
  {
    public static Type Type(this FieldInfo self) =>
      self.FieldType;
  }
}