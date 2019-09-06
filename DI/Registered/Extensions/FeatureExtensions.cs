using DI.Client;

namespace DI.Registered.Extensions
{
  internal static class FeatureExtensions
  {
    public static Registered<TFeature> Registered<TFeature>(this TFeature self) where TFeature : Feature =>
      new Registered<TFeature>(self);
  }
}