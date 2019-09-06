using System.Collections.Generic;
using FeaturesDI.Dependencies;

namespace FeaturesDI.Dependant
{
  internal interface IDependants
  {
    IDependantTypes Types { get; }

    void Add(object instance);
    void Remove(object instance);
    
    IEnumerable<object> ReleaseAll();
  }
}