using System;
using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public class JiraTask : IEquatable<JiraTask>
  {
    [Key(0)] public string Name;

    [Key(1)] public int DevDays;
    [Key(2)] public int QaDays;

    [Key(3)] public int DevProgress;
    [Key(4)] public int QaProgress;

    [IgnoreMember] public bool DevDone => DevProgress >= DevDays;
    [IgnoreMember] public bool QaDone => QaProgress >= QaDays;
    [IgnoreMember] public bool Complete => DevDone && QaDone;

    public JiraTask()
    {}

    public JiraTask(string name, int dev, int qa)
    {
      Name = name;
      DevDays = dev;
      QaDays = qa;
    }
    
    public bool Develop() => 
      ++DevProgress >= DevDays;

    public bool Test() =>
      ++QaProgress >= QaDays;

    public bool Equals(JiraTask other) =>
      DevDays == other?.DevDays &&
      QaDays == other.QaDays;

    public override int GetHashCode() => 
      DevDays.GetHashCode() ^ QaDays.GetHashCode();

    public override string ToString() =>
      $"{Name}: Dev: ({DevProgress} / {DevDays}) Qa: ({QaProgress} / {QaDays})";
  }
}