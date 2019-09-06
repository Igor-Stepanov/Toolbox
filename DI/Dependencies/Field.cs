using System;
using System.Reflection;

namespace FeaturesDI.Dependencies
{
  internal struct Field
  {
    public Type Type { get; }
    public FieldInfo Info { get; }

    public Field(FieldInfo info)
    {
      Info = info;
      Type = info.FieldType;
    }
    
    public InstanceField Of(object instance) =>
      new InstanceField(instance, this);
  }
}