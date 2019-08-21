using Sandbox.UDev.Correct.Liquid;

namespace Sandbox.UDev.Correct.Player
{
  public class Player : IPlayer
  {
    public float Hp { get; }
    public void Drink(ILiquid liquid)
    {
      throw new System.NotImplementedException();
    }

    public void Deal(float damage)
    {
      throw new System.NotImplementedException();
    }

    public void Heal(float hp)
    {
      throw new System.NotImplementedException();
    }
  }
}