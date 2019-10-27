namespace Gantt.Solutions
{
  public interface IGanttSolutions
  {
    GantSolution Best { get; }

    void Calculate(GantSolution solution);
    void Account(GantSolution solution);
  }
}