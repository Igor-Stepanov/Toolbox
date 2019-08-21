// ReSharper disable All
#pragma warning disable 649
using System;
using System.Collections.Generic;
using Sandbox.UDev.Correct.Liquid;
using Sandbox.UDev.Correct.Sound;
using Sandbox.UDev.Correct.Stages;
using Sandbox.UDev.Wrong;

namespace Sandbox.UDev.Correct.Bottle
{
  public class Bottle
  {
    private bool _openForbidden = false;

    public Bottle(IStages stages) =>
      stages.Changed += UpdateOpenAvailability;

    private void UpdateOpenAvailability(StageTypeId stageTypeId)
    {
      _openForbidden = stageTypeId == StageTypeId.ClanBoss
                    || stageTypeId == StageTypeId.Dungeon
                    || stageTypeId == StageTypeId.Story;

      if (SomeDebuffActive())
      {
        _openForbidden = false;
      }
      else if (SomeRuleNobodyCaresAbout())
      {
        var fakeOpened = StrangeBudinessRule1() && StrangeBudinessRule2();
        _openForbidden ^= fakeOpened;
      }
      
    }

    private bool SomeDebuffActive()
    {
      throw new NotImplementedException();
    }
    private bool SomeRuleNobodyCaresAbout()
    {
      throw new NotImplementedException();
    }
    private bool StrangeBudinessRule1()
    {
      throw new NotImplementedException();
    }
    private bool StrangeBudinessRule2()
    {
      throw new NotImplementedException();
    }
  }

  public class Durability
  {
  }

  public class BottleCap
  {
  }

  #pragma warning restore 649
}