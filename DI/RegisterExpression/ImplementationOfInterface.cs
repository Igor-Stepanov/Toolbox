namespace DI.RegisterExpression
{
  public struct ImplementationOf<TInterface>
                           where TInterface : IFeature { }

  public static class Implementation
  {
    public static ImplementationOf<TFeatureInterface> Of<TFeatureInterface>() where TFeatureInterface : IFeature
      => new ImplementationOf<TFeatureInterface>();
  }
}