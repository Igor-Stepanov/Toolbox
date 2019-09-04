using Common.Extensions;
using DI.Registered;

namespace DI.RegisterExpression
{
  public struct RegisterFeatureExpression<TFeature> where TFeature : Feature
  {
    private readonly TFeature _feature;
    private readonly RegisteredFeatures _registered;

    internal RegisterFeatureExpression(TFeature feature, RegisteredFeatures registered)
    {
      _feature = feature;
      _registered = registered;
    }

    public RegisterFeatureExpression<TFeature> AsSelf()
    {
      _registered.Add(_feature);
      return this;
    }

    internal RegisterFeatureExpression<TFeature> AsImplementationOf<TInterface>() where TInterface : class, IFeature
    {
      _registered.AddImplementationOf(_feature.As<TInterface>());
      return this;
    }
  }
  
  public static class RegisterFeatureExpressionExtension
  {
    public static RegisterFeatureExpression<TFeature> As<TFeature, TFeatureInterface>(this RegisterFeatureExpression<TFeature> self, ImplementationOf<TFeatureInterface> @interface)
      where TFeatureInterface : class, IFeature
      where TFeature : Feature, TFeatureInterface => 
      self.AsImplementationOf<TFeatureInterface>();
  }

}