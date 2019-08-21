using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Wrong
{
  public static class BottleOpener
  {
    public static ILiquid Open(BottleData bottleData)
    {
      if (bottleData.Liquid == null)
        return null;
      
      var liquid = bottleData.Liquid;
      bottleData.SetLiquid(null);

      return liquid;
    }
  }
}

