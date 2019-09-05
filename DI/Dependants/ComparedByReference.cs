using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DI.Dependants
{
  internal class Compared : IEqualityComparer<object>
  {
    private Compared()
    {
    }
    
    bool IEqualityComparer<object>.Equals(object x, object y) =>
      ReferenceEquals(x, y);
    
    /*
     * Default implementation, even if overriden. 
     * https://stackoverflow.com/questions/11240036/what-does-runtimehelpers-gethashcode-do
     */
    int IEqualityComparer<object>.GetHashCode(object o) => 
      RuntimeHelpers.GetHashCode(o);
    
    public static Compared ByReference() =>
      new Compared();
  }
}