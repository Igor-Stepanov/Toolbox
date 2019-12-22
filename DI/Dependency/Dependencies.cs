using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Reflection;
using DIFeatures.Dependency.Extensions;
using DIFeatures.Public;
using static System.Reflection.BindingFlags;

namespace DIFeatures.Dependency
{
  internal class Dependencies
  {
    private readonly Dictionary<Type, TypeDependencies> _dependencies =
      Types.WithAnyFieldWith<InjectAttribute>
        (Instance | NonPublic)
        .InCurrentDomain()
        .ToDictionary(x => x, x => x.Dependencies());


    public InstanceDependencies Of(object instance) =>
      DependenciesOf(instance.Type())
        .Within(instance);

    private TypeDependencies DependenciesOf(Type type) =>
      _dependencies[type];
  }
}