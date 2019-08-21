using System.Collections.Generic;
using System.Linq;
using Sandbox.UDev.Correct.Effects;

namespace Sandbox.UDev.Correct.Liquid
{
  public class Water : ILiquid
  {
    public IEnumerable<IEffect> Effects =>
      Enumerable.Empty<IEffect>();
  }
}