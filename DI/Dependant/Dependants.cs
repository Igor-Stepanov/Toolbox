using System;
using System.Collections.Generic;
using Common.Extensions;
using FeaturesDI.Client.Extensions;
using FeaturesDI.Dependant.Comparison;
using FeaturesDI.Dependency;
using FeaturesDI.Registered;

namespace FeaturesDI.Dependant
{
  internal class Dependants : IDependants
  {
    private readonly Dependencies _dependencies;
    private readonly HashSet<object> _dependants;

    public Dependants()
    {
      _dependencies = new Dependencies();
      _dependants = new HashSet<object>(Compared.ByReference());
    }

    public void Inject(object instance, IFeatures features)
    {
      if (!_dependants.Add(instance))
        throw new InvalidOperationException($"{instance.Type().Name} instance added already.");

      _dependencies
        .Of(instance.Type())
        .Within(instance)
        .InjectWith(features);
    }

    public void Release(object instance)
    {
      if (!_dependants.Remove(instance))
        throw new InvalidOperationException($"{instance.Type().Name} instance removed already.");

      _dependencies
        .Of(instance.Type())
        .Within(instance)
        .Release();
    }

    public void ReleaseAll()
    {
      _dependants.ForEach(x => x.ReleaseDependencies());
      _dependants.Clear();
    }
  }
}