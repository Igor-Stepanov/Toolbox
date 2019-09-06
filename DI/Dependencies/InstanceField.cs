using FeaturesDI.Client;

namespace FeaturesDI.Dependencies
{
  internal struct InstanceField
  {
    private readonly object _instance;
    private readonly Field _field;
    
    public InstanceField(object instance, Field field) =>
      (_instance, _field) =
      (instance, field);

    public void InjectWith(Feature implementation) =>
      Set(implementation);
    
    public void Release() =>
      Set(null);
    
    private void Set(IFeature value) =>
      _field.Info.SetValue(_instance, value);
  }
}