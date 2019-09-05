namespace Sandbox.UDev.Correct.Bottle
{
  public class BottleType
  {
    public BottleTypeId Id { get; }
    public BottleRarity Rarity { get; }
    public BottleSize Size { get; }

    public int UseExperience { get; }

    public BottleType(BottleTypeId id, BottleRarity rarity, BottleSize size, int openingExperience) =>
      (Id, Rarity, Size, UseExperience) =
      (id, rarity, size, openingExperience);

//    public Bottle CreateNew() =>
//      new Bottle(typeId: Id);
  }
}