using DI.Client;
using DI.Registered;

namespace DI.Dependencies
{
  internal struct InstanceDependency
  {
    private readonly object _instance;
    private readonly TypeDependency _typeDependency;
    
    public InstanceDependency(object instance, TypeDependency typeDependency) =>
      (_instance, _typeDependency) =
      (instance, typeDependency);

    public void Inject(IRegisteredFeatures registered) =>
      Set(registered.FeatureOf(_typeDependency.Type));
    
    public void Clear() =>
      Set(null);
    
    private void Set(IFeature value) =>
      _typeDependency.Field.SetValue(_instance, value);
  }
}