using System;
using System.Collections.Generic;

namespace GantFormula.Extensions
{
  public static class DeveloperExtensions
  {
    public static void For(this List<Developer> self, Action<Developer, int> action)
    {
      for (var i = 0; i < self.Count; i++)
        action?.Invoke(self[i], i);
    }
  }
}