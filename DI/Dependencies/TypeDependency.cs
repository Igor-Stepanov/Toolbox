using System;
using System.Reflection;

namespace DI.Dependencies
{
  internal struct TypeDependency
  {
    public Type Type { get; }
    public FieldInfo Field { get; }

    public TypeDependency(FieldInfo field)
    {
      Field = field;
      Type = field.GetType();
    }
    
    public InstanceDependency Of(object instance) =>
      new InstanceDependency(instance, this);
  }
}