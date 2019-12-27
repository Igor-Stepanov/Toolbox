using System;
using static System.AttributeTargets;

namespace DependencyInjection.API.Injection
{
  [AttributeUsage(Field)]
  public class InjectAttribute : Attribute
  {
  }
}