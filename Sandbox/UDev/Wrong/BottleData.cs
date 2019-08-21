using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Wrong
{
  public class BottleData
  {
    public ILiquid Liquid { get; private set; }

    public BottleData(ILiquid liquid = null) => 
      Liquid = liquid;

    public void SetLiquid(ILiquid liquid) =>
      Liquid = liquid;
  }
}