using System;

namespace DependencyInjection.Core.Exceptions
{
  internal interface IExceptions
  {
    void Handle(Exception exception);
  }
}