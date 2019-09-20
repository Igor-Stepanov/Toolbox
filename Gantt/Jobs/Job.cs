using System;
using Common.Hashes;
using MessagePack;
using static Gantt.Jobs.JobStatus;

namespace Gantt.Jobs
{
  [MessagePackObject]
  public struct Job : IEquatable<Job>
  {
    [Key(0)] public JobStatus Status;
    [Key(0)] public int Current;
    [Key(1)] public int Total;
    
    public Job(int total) => 
      Total = total;

    public JobStatus Perform()
    {
      Status = InProgress;
      
      if (++Current == Total)
        Status = Done;

      return Status;
    }

    public override bool Equals(object other) => 
      other is Job job && Equals(job);

    public override string ToString() => 
      $"{Current} / {Total}";

    public bool Equals(Job other) =>
      Total == other.Total;

    public override int GetHashCode() =>
      Hash.Of(Total);
  }
}