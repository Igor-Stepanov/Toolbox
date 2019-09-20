using System;
using Gantt.Jobs;
using MessagePack;
using static Gantt.Jobs.JobStatus;

namespace Gantt
{
  [MessagePackObject]
  public class JiraTask : IEquatable<JiraTask>
  {
    [Key(0)] public string Name;
    
    [Key(1)] public int? Assignee;
    
    [Key(1)] public Job DevJob;
    [Key(2)] public Job QaJob;

    [IgnoreMember] public bool Complete => 
      DevJob.Status == Done &&
      QaJob.Status == Done;

    public JiraTask(){}
    public JiraTask(string name, int dev, int qa)
    {
      Name = name;
      DevJob = new Job(dev);
      QaJob = new Job(qa);
    }

    public bool Equals(JiraTask other) =>
      DevJob.Total == other?.DevJob.Total &&
      QaJob.Total == other.QaJob.Total;
    
    public override int GetHashCode() => 
      DevJob.GetHashCode() ^ QaJob.GetHashCode();

    public override string ToString() =>
      $"{Name}: Dev: ({DevJob}) Qa: ({QaJob})";
  }
}