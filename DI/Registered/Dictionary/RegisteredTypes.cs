using System;
using System.Collections.Generic;

namespace DI.Registered.Dictionary
{
  public class RegisteredTypes
  {
    private readonly Type Type;
    private readonly HashSet<Type> Abstractions = new HashSet<Type>();

    public RegisteredTypes(Type type) =>
      Type = type;

    public void Add(Type abstraction)
    {
      if(!Abstractions.Add(abstraction))
        throw new InvalidOperationException($"{Type.Name} already registered as {abstraction.Name}");
    }

    public override int GetHashCode() => 
      Type.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is Type type && (Type == type || Abstractions.Contains(type));
    }

    public static implicit operator RegisteredTypes(Type type) =>
      new RegisteredTypes(type);
  }
}