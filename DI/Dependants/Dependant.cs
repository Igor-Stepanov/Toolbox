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
      var parameters = new object[] {request};
      
      var current = GetType();
      while (current != null)
      {
        current
          .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
          .SingleOrDefault(x => x.Has<DependenciesAttribute>())
          ?.Invoke(this, parameters);

        current = current.BaseType;
      }
    }

    public void ReleaseRequestedFeatures() =>
      _requestedFeatures.Release();
  }
}