using System;
using DI.Client;

namespace DI.Registered.Extensions
{
  internal static class RegisteredFeatureExtensions
  {
    public static Registered<TFeature> WhenFailed<TFeature>(this Registered<TFeature> self, Action<Exception> callback) where TFeature : Feature
    {
      self.Failed += callback;
      return self;
    }
  }
}