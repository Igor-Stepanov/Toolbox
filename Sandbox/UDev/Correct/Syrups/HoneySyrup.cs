using System.Collections.Generic;
using Sandbox.UDev.Correct.Effects;

namespace Sandbox.UDev.Correct.Syrups
{
  public class HoneySyrup : Syrup
  {
    protected override IEnumerable<IEffect> AdditionalEffects => new[]
    {
      new IncreasedStamina()
    };
  }
}