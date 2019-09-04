using System;

namespace DI.Registered.Extensions
{
  internal static class RegisteredFeaturesExtensions
  {
    public static RegisteredFeatures WhenFailed(this RegisteredFeatures self, Action<Exception> callback)
    {
      self.Failed += callback;
      return self;
    }

  }
}