using DIFeatures.Public;

namespace DIFeatures.Exceptions
{
  public interface IFeatureException
  {
    IFeature Feature { get; }
  }
}