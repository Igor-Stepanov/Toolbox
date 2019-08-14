using System.Collections.Generic;
using System.Linq;

namespace Sandbox.UDev.Correct.Liquid
{
  public class Water : ILiquid
  {
    public IEnumerable<IEffect> Buffs =>
      Enumerable.Empty<IEffect>();
  }
}