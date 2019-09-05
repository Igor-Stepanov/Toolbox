namespace Sandbox.UDev.Correct.Player.Extensions
{
  public static class PlayerExtensions
  {
    public static void Use(this IPlayer self, Bottle.Bottle bottle)
    {
      //var liquid = bottle.Open(); 
      self.Drink(/*liquid*/ null);
    }
  }
}

