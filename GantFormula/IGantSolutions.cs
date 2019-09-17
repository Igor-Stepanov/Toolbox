using GantFormula.Extensions;

namespace GantFormula
{
  public interface IGantSolutions
  {
    void Continue(GantSolution solution, Combination devCombination = null, Combination qaCombination = null);
  }
}