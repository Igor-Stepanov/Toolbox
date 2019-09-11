using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using DIFeatures.Dependant.Comparison;
using DIFeatures.Dependency;
using DIFeatures.Public.Extensions;
using DIFeatures.Registered;

namespace DIFeatures.Dependant
{
  internal class Dependants : IDependants
  {
    private readonly HashSet<object> _dependants;
    private readonly Dependencies _dependencies;

    public Dependants()
    {
      _dependants = new HashSet<object>(Compared.ByReference());
      _dependencies = new Dependencies();
    }

    public void Inject(object instance, IFeatures features)
    {
      if(!_dependants.Add(instance))
        throw new InvalidOperationException($"{instance.Type().Name} injected already.");
      
      _dependencies
        .Of(instance)
        .InjectWith(features);
    }

    public void Release(object instance)
    {
      if(!_dependants.Remove(instance))
        throw new InvalidOperationException($"{instance.Type().Name} released already.");
      
      _dependencies
        .Of(instance)
        .Release();
    }

    public void ReleaseAll()
    {
      _dependants
        .ToArray()
        .ForEach(Release);
    }
  }
}