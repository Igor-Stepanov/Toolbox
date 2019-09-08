using System.Reflection;
using DIFeatures.Dependency.Extensions;
using DIFeatures.Registered;

namespace DIFeatures.Dependency
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
      {
        var feature = features.ImplementationOf(field.Type());
        
        field.Of(_instance)
          .InjectWith(feature);
      }
    }

    public void Release()
    {
      foreach (var field in _fields)
        field.Of(_instance).Release();
    }
  }
}