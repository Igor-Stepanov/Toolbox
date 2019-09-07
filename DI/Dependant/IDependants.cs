using FeaturesDI.Registered;

namespace FeaturesDI.Dependant
{
  internal interface IDependants
  {
    void Inject(object instance, IFeatures features);
    void Release(object instance);
    
    void ReleaseAll();
  }
}