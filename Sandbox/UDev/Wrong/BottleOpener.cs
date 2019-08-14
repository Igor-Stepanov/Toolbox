using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Wrong
{
  public class BottleOpener
  {
    public static void Use(BottleData bottleData, IPlayer player)
    {
      if (bottleData.Liquid != null)
        player.Drink(bottleData.Liquid);
    }
  }
}