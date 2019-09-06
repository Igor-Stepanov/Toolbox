using FeaturesDI.Registered;

namespace FeaturesDI.Dependencies.Extensions
{
  internal static class ObjectExtensions
  {
    public static object InjectedWith(this object self, IFeatures features)
    {
      self.Dependencies().InjectWith(features);
      return self;
    }
    
    public static object Released(this object self)
    {
      self.Dependencies().Release();
      return self;
    }

    private static Dependant Dependencies(this object self) => self
     .GetType()
     .Dependencies()
     .Of(self);
  }
}