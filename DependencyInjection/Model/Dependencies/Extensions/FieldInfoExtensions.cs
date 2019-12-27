using System;
using System.Reflection;

namespace DependencyInjection.Model.Dependencies.Extensions
{
  public static class FieldInfoExtensions
  {
    public static Type Type(this FieldInfo self) =>
      self.FieldType;
  }
}