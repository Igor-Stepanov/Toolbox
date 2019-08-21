using System.Collections.Generic;
using Sandbox.UDev.Correct;
using Sandbox.UDev.Correct.Bottle;

namespace Sandbox.UDev.Wrong
{
  public class StaticData
  {
    public StaticBottleData Bottle;
  }

  public class StaticBottleData
  {
    public Dictionary<BottleRarity, int> ExperienceByRarity = new Dictionary<BottleRarity, int>
      {
        [BottleRarity.Rare] = 10,
        [BottleRarity.MediumRare] = 20,
        [BottleRarity.WellDone] = 30,
      };
    public Dictionary<BottleSize, int> ExperienceBySize  = new Dictionary<BottleSize, int>
      {
        [BottleSize.Small] = 10,
        [BottleSize.Medium] = 20,
        [BottleSize.Big] = 30,
      };
    
    public Dictionary<BottleTypeId, BottleType> Types = new Dictionary<BottleTypeId, BottleType>();

    public StaticBottleData()
    {
      New(BottleTypeId.Regular, BottleRarity.Rare, BottleSize.Medium);
      New(BottleTypeId.Magic, BottleRarity.Rare, BottleSize.Big);
      // ...
    }

    private void New(BottleTypeId typeId, BottleRarity rarity, BottleSize size)
    {
      var userExperience = ExperienceByRarity[rarity] 
                         + ExperienceBySize[size];
      
      var type = new BottleType(typeId, rarity, size, userExperience);
      
      Types.Add(typeId, type);
    }
  }
}