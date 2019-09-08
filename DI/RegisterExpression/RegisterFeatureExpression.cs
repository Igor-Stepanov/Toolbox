using System;
using DIFeatures.Errors;
using DIFeatures.Public;
using DIFeatures.Registered;

namespace DIFeatures.RegisterExpression
{
  public struct RegisterFeatureExpression<TFeature> where TFeature : Feature
  {
    private readonly IErrors _errors;
    private readonly TFeature _feature;
    private readonly IFeatures _features;

    internal RegisterFeatureExpression(IErrors errors, TFeature feature, IFeatures features)
    {
      _errors = errors;
      _feature = feature;
      _features = features;
    }

    public RegisterFeatureExpression<TFeature> AsSelf() =>
      AsImplementation(_feature.Type);

    internal RegisterFeatureExpression<TFeature> AsImplementation(Type abstraction)
    {
      try
      {
        _features.Register(abstraction, implementation: _feature);
      }
      catch (Exception exception)
      {
        _errors.Handle(exception);
      }
      
      return this;
    }
  }
  
  public static class RegisterFeatureExpressionExtension
  {
    public static RegisterFeatureExpression<TImplementation> AsImplementation<TAbstraction, TImplementation>
      (this RegisterFeatureExpression<TImplementation> self, TypeOf<TAbstraction> of)
        where TAbstraction : class, IFeature
        where TImplementation : Feature, TAbstraction => self.AsImplementation(typeof(TAbstraction));
  }
}