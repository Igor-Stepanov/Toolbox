using System;
using System.Linq;
using DI.Client;
using DI.Dependencies.Extensions;

namespace DI.Dependencies
{
  internal class TypeDependencies
  {
    private readonly Type _type;
    private readonly TypeDependency[] _dependencies;

    public TypeDependencies(Type type)
    {
      _type = type;
      _dependencies = _type.AllFieldsWith<InjectAttribute>()
       .AsDependencies()
       .ToArray();
    }
    
    public InstanceDependencies Of(object instance)
    {
      if (_type != instance.GetType())
        throw new InvalidOperationException($"Attempting to request {_type.Name} dependencies of {instance.GetType().Name} instance.");
      
      return new InstanceDependencies(instance, _dependencies);
    }
  }
}