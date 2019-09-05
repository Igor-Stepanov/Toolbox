using DI.Client;

namespace DI.Exceptions
{
  public interface IFeatureException
  {
    IFeature Feature { get; }
  }
}