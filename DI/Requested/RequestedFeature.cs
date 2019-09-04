using DI.Request;

namespace DI.Requested
{
  public class RequestedFeature<TReference> : IRequestedFeature where TReference : class, IFeature
  {    
    private readonly Feature<TReference> _reference;

    public RequestedFeature(Feature<TReference> container) => 
      _reference = container;

    public void ReleaseReference() => 
      _reference() = null;
  }
}