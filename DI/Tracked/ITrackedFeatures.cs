using DI.Requested;

namespace DI.Tracked
{
  public interface ITrackedFeatures
  {
    void Add(RequestedFeatures requested);
  }
}