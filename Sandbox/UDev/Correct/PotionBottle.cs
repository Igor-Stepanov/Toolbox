using System.Collections.Generic;
using Sandbox.UDev.Model.Liquid;

namespace Sandbox.UDev.Correct
{
  public class PotionBottle
  {
    public IPotion Potion { get; private set; }

    public PotionBottle(IPotion potion) => 
      Potion = potion;
    
    public void FillWith(IPotion potion)
    
    public IUsePotionResult Use()
    {
      var liquidBuffs = Potion.Buffs;
      Potion = null;
      return UsePotionResult.With(liquidBuffs);
    }
  }

  public class UsePotionResult : IUsePotionResult
  {
    
    
    public static UsePotionResult With(IEnumerable<IBuff> buffs)
    {
      
    }
  }
}