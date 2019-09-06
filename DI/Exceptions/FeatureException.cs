using FeaturesDI.Client;

namespace FeaturesDI.Exceptions
{
  public interface IFeatureException
  {
    IFeature Feature { get; }
  }
}