using Microsoft.Win32;
using Sandbox.UDev.Correct;
using Sandbox.UDev.Correct.Extensions;
using Sandbox.UDev.Correct.Liquid;
using Sandbox.UDev.Wrong;

namespace Sandbox
{
  internal class Program
  {
    private static Bottle _bottle = Bottle.Empty();
    private static BottleData _bottleData = new BottleData();
    
    public static void Main(string[] args)
    {
      var player = new Player();
      var water = new Water();
      
      player.Use(_bottle);
      BottleOpener.Use(_bottleData, player);
      
      _bottle.FillWith(water);
      //_bottleData.
        
      _bottle.FillWith(water);
      _bottleData.
    }
  }
}
