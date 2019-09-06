using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FeaturesDI.Dependant
{
  internal class Compared : IEqualityComparer<object>
  {
    private Compared() { }
    
    bool IEqualityComparer<object>.Equals(object x, object y) =>
      ReferenceEquals(x, y);
     
    int IEqualityComparer<object>.GetHashCode(object o) => 
      RuntimeHelpers.GetHashCode(o); // ThanksHelper.TryDo();
    
    public static Compared ByReference() =>
      new Compared();
  }
}