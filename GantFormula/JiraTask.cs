using System;

namespace GantFormula
{
  public class JiraTask : IEquatable<JiraTask>
  {
    public bool Unassigned => Assignee == null;

    public string Name { get; set; }
    public IWorker Assignee { get; set; }
    public Status Status { get; set; }
    
    public int DevelopmentDays { get; set; }
    public int QaDays { get; set; }

    public int DevelopmentProgress { get; set; }
    public int QaProgress { get; set; }

    public Status Develop()
    {
      DevelopmentProgress++;
      if (DevelopmentProgress >= DevelopmentDays)
        Status = Status.ReadyForQa;

      return Status;
    }
    
    public Status AssureQuality()
    {
      QaProgress++;
      if (QaProgress >= QaDays)
        Status = Status.Done;

      return Status;
    }

    public void ReadyForQa()
    {
      Status = Status.ReadyForQa;
      Assignee = null;
    }

    public void Done()
    {
      Status = Status.Done;
      Assignee = null;
    }

    public bool Equals(JiraTask other) =>
      DevelopmentDays == other?.DevelopmentDays &&
      QaDays == other.QaDays;

    public override int GetHashCode() => 
      DevelopmentDays.GetHashCode() ^ QaDays.GetHashCode();

    public override string ToString() =>
      $"{Name} {Status} D: {DevelopmentProgress}/{DevelopmentDays} Q: {QaProgress}/{QaDays} A: {Assignee != null}";
  }
}