namespace DI.RegisterExpression
{
  // Pass type of abstract feature implementation using static import.
  public class Implementation
  {
    public static TypeOf<TAbstractFeature> Of<TAbstractFeature>()
      where TAbstractFeature : IFeature => null;
  }

  public delegate void TypeOf<TFeature>()
    where TFeature : IFeature;
}