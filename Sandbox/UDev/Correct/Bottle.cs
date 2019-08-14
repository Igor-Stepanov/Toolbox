using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Correct
{
  public class Bottle
  {
    public ILiquid Liquid { get; private set; }

    public Bottle(ILiquid liquid = null) => 
      Liquid = liquid;

    public void FillWith(ILiquid liquid) =>
      Liquid = liquid;
    
    public ILiquid Open()
    {
      var liquid = Liquid;
      Liquid = null;
      return liquid;
    }
    
    public void Pour() =>
      Liquid = null;

    public static Bottle Empty() =>
      new Bottle();
  }
}