using Gantt.Extensions;
using Gantt.Solutions;

namespace Gantt
{
  public interface IGanttSolutions
  {
    GantSolution Best { get; }

    void Continue(GantSolution solution, Combination combination = null);
    void Add(GantSolution solution);
  }
}