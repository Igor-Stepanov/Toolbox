using System.Reflection;
using DIFeatures.Public;

namespace DIFeatures.Dependency
{
  internal struct InstanceField
  {
    private readonly object _instance;
    private readonly FieldInfo _field;
    
    public InstanceField(object instance, FieldInfo field) =>
      (_instance, _field) =
      (instance, field);

    public void InjectWith(Feature implementation) =>
      Set(implementation);
    
    public void Release() =>
      Set(null);
    
    private void Set(IFeature value) =>
      _field.SetValue(_instance, value);
  }
}