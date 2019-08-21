using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Correct.Syrups.Extensions
{
  public static class LiquidExtensions
  {
    public static ILiquid With<TSyrup>(this ILiquid self)
      where TSyrup : Syrup, new() =>
      new TSyrup().MixedWith(self);
  }
}

