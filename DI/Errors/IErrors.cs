using System;

namespace DIFeatures.Errors
{
  public interface IErrors
  {
    void Handle(Exception exception);
  }
}