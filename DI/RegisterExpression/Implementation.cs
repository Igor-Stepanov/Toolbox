using FeaturesDI.Client;

namespace FeaturesDI.RegisterExpression
{
  // Pass type of abstract feature implementation using static import.
  public static class Implementation
  {
    public static TypeOf<TAbstractFeature> Of<TAbstractFeature>() where TAbstractFeature : IFeature =>
      null;
  }

  public interface TypeOf<TFeature> where TFeature : IFeature { }
}