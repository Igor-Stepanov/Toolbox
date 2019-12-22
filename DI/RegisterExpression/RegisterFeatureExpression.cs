using DIFeatures.Public;
using DIFeatures.Registered;

namespace DIFeatures.RegisterExpression
{
  public struct RegisterFeatureExpression<TFeature> where TFeature : IFeature
  {
    private readonly IFeatures _features;

    internal RegisterFeatureExpression(IFeatures features) =>
      _features = features;

    public void Be<TImplementation>() where TImplementation : Feature, TFeature, new() =>
      _features.Register(typeof(TFeature), typeof(TImplementation));
  }
}