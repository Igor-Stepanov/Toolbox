namespace Sandbox.UDev.Correct.Bottle.Extensions
{
  public static class BottleExtensions
  {
    public static bool HasLiquid(this Bottle self) =>
      self.Liquid != null;
  }
}