using System;
using Gantt.Jobs;
using MessagePack;
using static Gantt.Jobs.JobStatus;

namespace Gantt.Tasks
{
  [MessagePackObject]
  public class JiraTask : IEquatable<JiraTask>
  {
    [Key(0)] public string Name;
    [Key(1)] public int? Assignee;
    [Key(2)] public Job DevJob;
    [Key(3)] public Job QAJob;

    [IgnoreMember] public bool Complete => 
      DevJob.Status == Done &&
      QAJob.Status == Done;

    public JiraTask(){}
    public JiraTask(string name, int dev, int qa)
    {
      Name = name;
      DevJob = new Job {Amount = dev};
      QAJob = new Job {Amount = qa};
    }

    public bool Equals(JiraTask other) =>
      DevJob.Equals(other?.DevJob) &&
      QAJob.Equals(other?.QAJob);
    
    public override int GetHashCode() => 
      DevJob.GetHashCode() ^ QAJob.GetHashCode();

    public override string ToString() =>
      $"{Name}: Dev: ({DevJob}) QA: ({QAJob})";
  }
}