using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using FeaturesDI.Dependencies.Extensions;

namespace FeaturesDI.Dependant
{
  internal interface IDependants
  {
    void Add(object instance);
    void Remove(object instance);
    
    IEnumerable<object> ReleaseAll();
  }

  internal class Dependants : IDependants
  {
    private readonly HashSet<object> _dependants = new HashSet<object>(Compared.ByReference());

    public void Add(object instance)
    {
      if (!_dependants.Add(instance))
        throw new InvalidOperationException($"{instance.GetType().Name} instance added already.");
    }

    public void Remove(object instance)
    {
      if (!_dependants.Remove(instance))
        throw new InvalidOperationException($"{instance.GetType().Name} instance removed already.");
    }

    public IEnumerable<object> ReleaseAll() => 
      _dependants
       .ToArray()
       .ForEach(x => _dependants.Remove(x.Released()));
  }
}