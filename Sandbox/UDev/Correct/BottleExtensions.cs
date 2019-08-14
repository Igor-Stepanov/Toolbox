namespace Sandbox.UDev.Correct
{
  public static class BottleExtensions
  {
    public static bool HasLiquid(this Bottle self) =>
      self.Liquid != null;
  }
}