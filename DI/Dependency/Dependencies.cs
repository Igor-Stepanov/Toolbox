using System;
using System.Collections.Generic;
using Common.Extensions;
using DIFeatures.Dependency.Extensions;

namespace DIFeatures.Dependency
{
  internal class Dependencies
  {
    private readonly Dictionary<Type, TypeDependencies> _dependencies = new Dictionary<Type, TypeDependencies>();

    public InstanceDependencies Of(object instance) =>
      DependenciesOf(instance.Type())
        .Within(instance);

    private TypeDependencies DependenciesOf(Type type)
    {
      if (_dependencies.TryGetValue(type, out var typeDependencies))
        return typeDependencies;

      typeDependencies = type.Dependencies();
      _dependencies.Add(type, typeDependencies);

      return typeDependencies;
    }
  }
}