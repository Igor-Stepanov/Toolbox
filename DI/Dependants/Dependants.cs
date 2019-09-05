using System.Collections.Generic;

namespace DI.Dependants
{
  public class Dependants
  {
    private HashSet<object> _dependants = new HashSet<object>(Compared.ByReference());
  }
}