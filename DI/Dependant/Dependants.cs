using System;
using System.Linq;
using Common.Collections.ConcurrentHashSet;
using Common.Extensions;
using DIFeatures.Dependant.Comparison;
using DIFeatures.Dependency;
using DIFeatures.Registered;

namespace DIFeatures.Dependant
{
  internal class Dependants : IDependants
  {
    private readonly ConcurrentHashSet<object> _dependants;
    private readonly Dependencies _dependencies;

    public Dependants()
    {
      _dependants = new ConcurrentHashSet<object>(Compared.ByReference());
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
        .Select(x => x.Key)
        .ForEach(Release);
    }
  }
}