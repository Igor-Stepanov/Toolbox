using System;
using Common.Collections.ConcurrentHashSet;
using Common.Equality;
using Common.Extensions;
using DependencyInjection.Core;
using DependencyInjection.Core.Dependencies;
using DependencyInjection.Core.Exceptions;

namespace DependencyInjection.Model
{
  internal class Dependants : IDependants
  {
    private readonly IDependencies _dependencies;
    private readonly IFeatures _features;
    private readonly IExceptions _exceptions;

    private readonly ConcurrentHashSet<object> _dependants;

    public Dependants(IDependencies dependencies, IFeatures features, IExceptions exceptions)
    {
      _dependencies = dependencies;
      _features = features;
      _exceptions = exceptions;

      _dependants = new ConcurrentHashSet<object>(Compared.ByReference());
    }

    public void Inject(object instance) =>
      _exceptions.Try(() =>
      {
        if (!_dependants.Add(instance))
          throw new InvalidOperationException($"{instance.Type().Name} injected already.");

        _dependencies
         .Of(instance)
         .InjectWith(_features);
      });

    public void Release(object instance) =>
      _exceptions.Try(() =>
      {
        if (!_dependants.Remove(instance))
          throw new InvalidOperationException($"{instance.Type().Name} released already.");

        _dependencies
         .Of(instance)
         .Release();
      });

    public void Clear() =>
      _exceptions.Try(() =>
        _dependants
         .ToArray()
         .ForEach(Release));
  }
}