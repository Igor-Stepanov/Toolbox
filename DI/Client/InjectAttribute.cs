using System;

namespace DI.Client
{
  [AttributeUsage(AttributeTargets.Field)]
  public class InjectAttribute : Attribute
  {
  }
}