using System;
using static System.AttributeTargets;

namespace DI.Dependants
{
  [AttributeUsage(Method)]
  public class DependenciesAttribute : Attribute
  {
  }
}