using System;
using System.Runtime.CompilerServices;

namespace DI.Dependants
{
  public class Dependant : IEquatable<Dependant>
  {
    private readonly object _instance;

    public Dependant(object instance)
    {
      _instance = instance;
      RuntimeHelpers.GetHashCode()
    }

    public bool Equals(Dependant other) => 
      ReferenceEquals(_instance, other?._instance);

    public override bool Equals(object other)
    {
      if (ReferenceEquals(null, other))
        return false;
      
      if (ReferenceEquals(this, other))
        return true;
      
      if (other.GetType() != GetType())
        return false;
      
      return Equals((Dependant) other);
    }

    public override int GetHashCode() =>
      _instance.GetHashCode();
  }
}