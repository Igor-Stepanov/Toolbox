using System;
using System.Collections.Generic;

namespace FeaturesDI.Dependencies
{
  internal class DependantTypes : IDependantTypes
  {
    private readonly Dictionary<Type, DependantType> _dependencies = new Dictionary<Type, DependantType>();

    public DependantType OneOf(Type type)
    {
      if (_dependencies.TryGetValue(type, out var dependencies))
        return dependencies;
      
      dependencies = new DependantType(type);
      _dependencies.Add(type, dependencies);

      return dependencies;
    }
  }
}