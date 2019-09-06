using System;
using Common.Extensions;
using DI.Client;
using DI.Registered;

namespace DI.RegisterExpression
{
  public struct RegisterFeatureExpression<TFeature> where TFeature : Feature
  {
    private readonly TFeature _feature;
    private readonly IRegisteredFeatures _registered;

    internal RegisterFeatureExpression(TFeature feature, IRegisteredFeatures registered)
    {
      _feature = feature;
      _registered = registered;
    }

    public RegisterFeatureExpression<TFeature> AsSelf()
    {
      _registered.Add(_feature);
      return this;
    }

    internal RegisterFeatureExpression<TFeature> AsImplementationOf(Type abstractionType)
    {
      _registered.AddImplementationOf(abstractionType, implementation: _feature);
      return this;
    }
  }
  
  public static class RegisterFeatureExpressionExtension
  {
    public static RegisterFeatureExpression<TFeature> As<TFeature, TAbstraction>
    (this RegisterFeatureExpression<TFeature> self, TypeOf<TAbstraction> type)
      where TAbstraction : class, IFeature
      where TFeature : Feature, TAbstraction => 
      self.AsImplementationOf(typeof(TAbstraction));
  }
}