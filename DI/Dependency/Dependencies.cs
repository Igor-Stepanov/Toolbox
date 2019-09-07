using System;
using System.Collections.Generic;
using FeaturesDI.Dependency.Extensions;

namespace FeaturesDI.Dependency
{
  internal class Dependencies
  {
    private readonly Dictionary<Type, TypeDependencies> _dependencies = new Dictionary<Type, TypeDependencies>();

    public TypeDependencies Of(Type type)
    {
      if (_dependencies.TryGetValue(type, out var dependencies))
        return dependencies;

      dependencies = type.Dependencies();
      _dependencies.Add(type, dependencies);

      return dependencies;
    }
  }
}