using System.Collections.Generic;
using System.Linq;
using Sandbox.UDev.Correct.Effects;
using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Correct.Syrups
{
  public abstract class Syrup : ILiquid
  {
    public IEnumerable<IEffect> Effects => _other.Effects.Union(AdditionalEffects);

    protected abstract IEnumerable<IEffect> AdditionalEffects { get; }

    private ILiquid _other;
    
    public ILiquid MixedWith(ILiquid other)
    {
      _other = other;
      return this;
    }
  }
}


