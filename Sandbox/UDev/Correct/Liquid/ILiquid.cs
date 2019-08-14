using System.Collections.Generic;

namespace Sandbox.UDev.Correct.Liquid
{
  public interface ILiquid
  {
    IEnumerable<IEffect> Buffs { get; }
  }
}