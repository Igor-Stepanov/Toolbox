using System;
using static System.Activator;

namespace DI.FeatureInstances
{
  internal class LazyFeatureInstance : IFeatureInstance
  {
    public object Value =>
      _instance ?? (_instance = CreateInstance(_type));
    
    private object _instance;
    private readonly Type _type;

    public LazyFeatureInstance(Type type) => 
      _type = type;
  }
}