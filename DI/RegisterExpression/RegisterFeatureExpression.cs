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

    internal RegisterFeatureExpression(IErrors errors, IFeatures features, TFeature feature)
    {
      _errors = errors;
      _feature = feature;
      _features = features;
    }

    public RegisterFeatureExpression<TFeature> AsSelf() =>
      AsImplementationOf(_feature.Type);

    internal RegisterFeatureExpression<TFeature> AsImplementationOf(Type abstraction)
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
        where TImplementation : Feature, TAbstraction => self.AsImplementationOf(typeof(TAbstraction));
  }
}