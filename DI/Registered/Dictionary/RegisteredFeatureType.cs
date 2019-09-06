using System;
using System.Collections.Generic;

namespace DI.Registered.Dictionary
{
  public class RegisteredFeatureType
  {
    private readonly Type MainType;
    private readonly HashSet<Type> Abstractions = new HashSet<Type>();

    public RegisteredFeatureType(Type type) =>
      MainType = type;

    public void Add(Type abstraction)
    {
      if(!Abstractions.Add(abstraction))
        throw new InvalidOperationException($"{MainType.Name} already registered as {abstraction.N}");
    }

    public override int GetHashCode() => 
      MainType.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is Type type && (MainType == type || Abstractions.Contains(type));
    }

    public static implicit operator RegisteredFeatureType(Type type) =>
      new RegisteredFeatureType(type);
  }
}