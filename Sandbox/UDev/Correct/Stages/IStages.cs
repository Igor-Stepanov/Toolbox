using System;

namespace Sandbox.UDev.Correct.Stages
{
  public interface IStages
  {
    event Action<StageTypeId> Changed;
  }
}