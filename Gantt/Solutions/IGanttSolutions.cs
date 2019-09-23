using Gantt.Combinations;

namespace Gantt.Solutions
{
  public interface IGanttSolutions
  {
    GantSolution Best { get; }

    void Continue(GantSolution solution, Combination combination);
    void Add(GantSolution solution);
  }
}