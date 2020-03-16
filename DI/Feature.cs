using System.Collections.Generic;
using System.Linq;
using DI.FeatureInstances;

namespace DI
{
  internal class Feature
  {
    private readonly List<IFeatureInstance> _instances
      = new List<IFeatureInstance>();

    public void Register(IFeatureInstance instance) =>
      _instances.Add(instance);

    public T Resolve<T>() =>
      (T) _instances[0].Value;

    public IEnumerable<T> ResolveAll<T>() =>
      (IEnumerable<T>) _instances.Select(x => x.Value);
  }
}