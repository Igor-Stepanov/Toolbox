using System.Collections.Generic;
using Sandbox.UDev.Model.Liquid;

namespace Sandbox.UDev.Wrong
{
  public class PotionData
  {
    IEnumerable<IBuff> Buffs { get; }

    public PotionData(IEnumerable<IBuff> buffs) => 
      Buffs = buffs;

    
    /* public void SetPotion(IPotion other) =>
      _potion = other; как вы будете его заполнять? вот так:*/
  }
}