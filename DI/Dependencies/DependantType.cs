using System;
using System.Linq;
using FeaturesDI.Client;
using FeaturesDI.Dependencies.Extensions;

namespace FeaturesDI.Dependencies
{
  internal class DependantType
  {
    private readonly Type _type;
    private readonly Field[] _dependencies;

    public DependantType(Type type)
    {
      _type = type;
      _dependencies = _type.FieldsWith<InjectAttribute>().ToArray();
    }
    
    public Dependant Of(object instance)
    {
      if (_type != instance.GetType())
        throw new InvalidOperationException($"Attempting to request {_type.Name} dependencies of {instance.GetType().Name} instance.");
      
      return new Dependant(instance, _dependencies);
    }
  }
}