using System;
using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public class WorkDay
  {
    [Key(0)] public DateTime Day { get; set; }
    [Key(1)] public string Task { get; set; }
  }
}