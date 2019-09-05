using System;
using System.Collections.Generic;
using System.Linq;
using Common.Reflection.Extensions;
using DI.Client;
using DI.Dependants;
using DI.Dependencies.Extensions;
using DI.Request;
using static System.Reflection.BindingFlags;

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