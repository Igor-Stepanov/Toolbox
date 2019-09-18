using GantFormula.Extensions;

namespace GantFormula
{
  public interface IGant
  {
    GantSolution Best { get; }

    void Continue(GantSolution solution, Combination devCombination = null, Combination qaCombination = null);
    void Add(GantSolution solution);
  }
}