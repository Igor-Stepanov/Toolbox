using System;
using System.Collections.Generic;

namespace DI.Public
{
  public class Container
  {
    private static Features _features;
    private bool _initialized;
    
    public Container()
    {
      if (_features != null)
        throw new InvalidOperationException($"Cannot create two instances of {nameof(Container)}");
      
      _features = new Features();
    }
    
    public RegisterFeatureExpression<TFeature> Let<TFeature>() where TFeature : class
    {
      if (_initialized)
        throw new InvalidOperationException("Container already initialized.");
      
      return new RegisterFeatureExpression<TFeature>(_features);
    }

    public void Initialize() =>
      _initialized = true;

    internal static T Resolve<T>() =>
      _features.Resolve<T>();
    
    internal static IEnumerable<T> ResolveAll<T>() =>
      _features.ResolveAll<T>();

    public void Clear()
    {
      _features.Clear();
      _features = null;

      _initialized = false;
    }
  }
}