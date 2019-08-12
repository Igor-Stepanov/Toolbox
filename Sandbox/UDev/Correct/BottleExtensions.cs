namespace Sandbox.UDev.Correct
{
  public static class BottleExtensions
  {
    public static bool HasLiquid(this PotionBottle self) =>
      self.Potion != null;
  }
}