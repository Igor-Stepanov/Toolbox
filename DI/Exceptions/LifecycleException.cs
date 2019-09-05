using System;

namespace DI.Exceptions
{
  public class LifecycleException : Exception
  {
    public LifecycleException(Exception exception) : base(nameof(LifecycleException), exception)
    {
    }
  }
}