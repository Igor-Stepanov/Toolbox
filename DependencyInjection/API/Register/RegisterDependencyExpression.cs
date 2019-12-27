using DependencyInjection.API.Dependencies;
using DependencyInjection.Core;

namespace DependencyInjection.API.Register
{
  public struct RegisterDependencyExpression<TDependency> where TDependency : IDependency
  {
    private readonly IFeatures _features;

    internal RegisterDependencyExpression(IFeatures features) =>
      _features = features;

    public void Be<TFeature>() where TFeature : Feature, TDependency, new() =>
      _features.Register(typeof(TDependency), typeof(TFeature));
  }
}