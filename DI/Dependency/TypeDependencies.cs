using System;
using System.Linq;
using System.Reflection;
using Common.Extensions;
using FeaturesDI.Client;
using FeaturesDI.Dependency.Extensions;

namespace FeaturesDI.Dependency
{
  internal class TypeDependencies
  {
    private readonly Type _type;
    private readonly FieldInfo[] _dependencies;

    public TypeDependencies(Type type)
    {
      _type = type;
      _dependencies = _type.FieldsWith<InjectAttribute>().ToArray();
    }
    
    public InstanceDependencies Within(object instance)
    {
      if (_type != instance.Type())
        throw new InvalidOperationException($"{instance.Type().Name} instance requests {_type.Name} dependencies.");

      return new InstanceDependencies(instance, _dependencies);
    }
  }
}