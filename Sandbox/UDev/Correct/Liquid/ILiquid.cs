using System.Collections.Generic;
using Sandbox.UDev.Correct.Effects;

namespace Sandbox.UDev.Correct.Liquid
{
  public interface ILiquid
  {
    IEnumerable<IEffect> Effects { get; }
  }
}