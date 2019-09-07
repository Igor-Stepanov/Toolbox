using System.Reflection;
using FeaturesDI.Dependency.Extensions;
using FeaturesDI.Registered;

namespace FeaturesDI.Dependency
{
  internal struct InstanceDependencies
  {
    private readonly object _instance;
    private readonly FieldInfo[] _fields;

    public InstanceDependencies(object instance, FieldInfo[] fields)
    {
      _instance = instance;
      _fields = fields;
    }
    
    public void InjectWith(IFeatures features)
    {
      foreach (var field in _fields)
        field.Of(_instance)
          .InjectWith(features.ImplementationOf(field.FieldType));
    }

    public void Release()
    {
      foreach (var field in _fields)
        field.Of(_instance).Release();
    }
  }
}