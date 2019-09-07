using FeaturesDI.Client;

namespace FeaturesDI.RegisterExpression
{
  public delegate void TypeOf<TAbstraction>() where TAbstraction : IFeature;

  // Use import static
  public static class Implementation
  {
    // Pass abstraction type using generic type parameter.
    public static TypeOf<TAbstraction> Of<TAbstraction>() where TAbstraction : IFeature =>
      null;
  }
}