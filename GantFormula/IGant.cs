using GantFormula.Extensions;

namespace GantFormula
{
  public interface IGant
  {
    GantSolution Solution { get; }

    void Continue(GantSolution solution, Combination devCombination = null, Combination qaCombination = null);
    void Add(GantSolution solution);
  }
}