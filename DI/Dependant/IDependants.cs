using DIFeatures.Registered;

namespace DIFeatures.Dependant
{
  internal interface IDependants
  {
    void Inject(object instance, IFeatures features);
    void Release(object instance);
    
    void ReleaseAll();
  }
}