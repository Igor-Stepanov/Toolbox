namespace Sandbox.UDev.Correct.Liquid
{
  public interface IPlayer
  {
    float Hp { get; }

    void Drink(ILiquid liquid);

    void Deal(float damage);
    void Heal(float hp);
  }
}