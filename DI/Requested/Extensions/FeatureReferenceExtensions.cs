using DI.Request;

namespace DI.Requested.Extensions
{
  public static class FeatureReferenceExtensions
  {
    public static IRequestedFeature Requested<TReference>(this Feature<TReference> self) where TReference : class, IFeature =>
      new RequestedFeature<TReference>(self);
  }
}