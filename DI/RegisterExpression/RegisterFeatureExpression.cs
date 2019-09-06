using System;
using FeaturesDI.Client;
using FeaturesDI.Registered;

namespace FeaturesDI.RegisterExpression
{
  public struct RegisterFeatureExpression<TFeature> where TFeature : Feature
  {
    private readonly IFeatures _registered;

    internal RegisterFeatureExpression(IFeatures registered) => 
      _registered = registered;

    public RegisterFeatureExpression<TFeature> AsSelf()
    {
      _registered.Add(typeof(TFeature), typeof(TFeature));
      return this;
    }

    internal RegisterFeatureExpression<TFeature> As(Type abstraction)
    {
      _registered.Add(abstraction, typeof(TFeature));
      return this;
    }
  }
  
  public static class RegisterFeatureExpressionExtension
  {
    public static RegisterFeatureExpression<TImplementation> As<TAbstraction, TImplementation>(this RegisterFeatureExpression<TImplementation> self, TypeOf<TAbstraction> type)
      where TAbstraction : class, IFeature
      where TImplementation : Feature, TAbstraction => 
      self.As(typeof(TAbstraction));
  }
}