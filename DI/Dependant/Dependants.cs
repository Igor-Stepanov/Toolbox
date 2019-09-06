using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using DI.Dependencies.Extensions;
using DI.Logs;
using DI.Registered;

namespace DI.Dependant
{
  internal class Dependants
  {
    private readonly HashSet<object> _dependants = new HashSet<object>(Compared.ByReference());

    private readonly ILog _log;
    private readonly IRegisteredFeatures _registeredFeatures;

    public Dependants(ILog log, IRegisteredFeatures registeredFeatures)
    {
      _log = log;
      _registeredFeatures = registeredFeatures;
    }

    public void Add(object instance)
    {
      if (!_dependants.Add(instance))
      {
        _log.Warning($"Duplicate injection of {instance.GetType().Name} instance.");
        return;
      }

      instance.Dependencies().InjectWith(_registeredFeatures);
    }

    public void Release(object instance)
    {
      if (!_dependants.Remove(instance))
      {
        _log.Warning($"{instance.GetType().Name} instance released already.");
        return;
      }
      
      instance.Dependencies().Clear();
    }
    
    public void Release() => _dependants.ToArray()
      .ForEach(leaked =>
      {
        leaked.Dependencies().Clear();
        _dependants.Remove(leaked);
            
        _log.Warning($"{leaked.GetType().Name} instance was not released.");
      });
  }
}