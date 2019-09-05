using System.Linq;
using System.Reflection;
using Common.Reflection.Extensions;
using DI.Request;
using DI.Requested;

namespace DI.Dependants
{
  public class Dependant
  {
    private readonly RequestedFeatures _requestedFeatures;

    public Dependant()
    {
      var request = Features.Request();
      Use(request);
      _requestedFeatures = request.Close();
    }
    
    private void Use(FeatureRequest request)
    {
      GetType()
        .Dependencies()
        .Inject(this)
        .Using(request);
      
      foreach (var reference in featureReferences)
      {
        
      }
      request.Obtain()
      
      var parameters = new object[] {request};
      
      
      }
    }

    public void ReleaseRequestedFeatures() =>
      _requestedFeatures.Release();
  }
}