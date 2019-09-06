using Common.Extensions;
using DI.Dependencies.Extensions;
using DI.Registered;

namespace DI.Dependencies
{
  internal struct InstanceDependencies
  {
    private readonly object _instance;
    private readonly TypeDependency[] _typeDependencies;

    public InstanceDependencies(object instance, TypeDependency[] typeDependencies)
    {
      _instance = instance;
      _typeDependencies = typeDependencies;
    }
    
    public void InjectWith(IRegisteredFeatures registeredFeatures) => 
      _typeDependencies
       .Of(_instance)
       .ForEach(x => x.Inject(registeredFeatures));

    public void Clear() => 
      _typeDependencies
       .Of(_instance)
       .ForEach(x => x.Clear());
  }
}