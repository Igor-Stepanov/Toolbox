using DI.FeatureInstances;

namespace DI.Public
{
  public struct RegisterFeatureExpression<TFeature> where TFeature : class
  {
    private readonly Features _features;

    internal RegisterFeatureExpression(Features features) =>
      _features = features;

    public void Be<TImplementation>() where TImplementation : class, TFeature, new() =>
      _features.Register(typeof(TFeature), typeof(TImplementation));

    public void BeUnique<TImplementation>() where TImplementation : class, TFeature, new() =>
      _features.RegisterUnique(typeof(TFeature), new FeatureInstance(typeof(TImplementation)));

    public void BeLazy<TImplementation>() where TImplementation : class, TFeature, new() =>
      _features.RegisterUnique(typeof(TFeature), new LazyFeatureInstance(typeof(TImplementation)));
  }
}