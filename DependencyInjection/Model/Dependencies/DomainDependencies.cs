using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Reflection;
using DependencyInjection.API.Injection;
using DependencyInjection.Core.Dependencies;
using DependencyInjection.Model.Dependencies.Extensions;
using static System.Reflection.BindingFlags;

namespace DependencyInjection.Model.Dependencies
{
  internal class DomainDependencies : IDependencies
  {
    private readonly Dictionary<Type, TypeDependencies> _dependencies =
      Types.WithAnyFieldWith<InjectAttribute>
          (Instance | NonPublic)
       .InCurrentDomain()
       .ToDictionary(x => x, x => x.Dependencies());


    InstanceDependencies IDependencies.Of(object instance) =>
      DependenciesOf(instance.Type())
        .Within(instance);
    
    void IDependencies.Clear() =>
      _dependencies.Clear();

    private TypeDependencies DependenciesOf(Type type) =>
      _dependencies[type];
  }
}