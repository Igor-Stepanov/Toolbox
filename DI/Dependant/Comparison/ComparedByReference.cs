using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace DIFeatures.Dependant.Comparison
{
  internal class Compared : IEqualityComparer<object>
  {
    private Compared() { }
    
    bool IEqualityComparer<object>.Equals(object x, object y) =>
      ReferenceEquals(x, y);
     
    int IEqualityComparer<object>.GetHashCode(object o) => 
      RuntimeHelpers.GetHashCode(o);
    
    public static Compared ByReference() =>
      new Compared();
  }
}