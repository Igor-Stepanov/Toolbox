using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Correct.Extensions
{
  public static class UserExtensions
  {
    public static void Use(this IPlayer self, Bottle bottle)
    {
      self.Drink(bottle.Open());
    }
  }
}