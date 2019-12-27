using System.Collections.Generic;
using Initial = System.Runtime.CompilerServices.RuntimeHelpers;

namespace Common.Equality
{
  public class Compared : IEqualityComparer<object>
  {
    private Compared() { }
    
    bool IEqualityComparer<object>.Equals(object x, object y) =>
      ReferenceEquals(x, y);

    int IEqualityComparer<object>.GetHashCode(object o) =>
      ReferenceHashCode(o);
    
    public static Compared ByReference() =>
      new Compared();

    private static int ReferenceHashCode(object o) =>
      Initial.GetHashCode(o);
  }
}