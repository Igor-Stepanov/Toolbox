using System.Collections.Generic;
using Sandbox.UDev.Correct;

namespace Sandbox.UDev.Model.Liquid
{
  public interface IPotion
  {
    IEnumerable<IBuff> Buffs { get; }
  }
}