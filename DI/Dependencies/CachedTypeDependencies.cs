using System;
using System.Collections.Generic;

namespace FeaturesDI.Dependencies
{
  internal static class CachedTypeDependencies
  {
    private static readonly Dictionary<Type, TypeDependencies> Cache = new Dictionary<Type, TypeDependencies>();
    
    public static TypeDependencies Of(Type type)
    {
      if (Cache.TryGetValue(type, out var dependencies))
        return dependencies;
      
      dependencies = new TypeDependencies(type);
      Cache.Add(type, dependencies);

      return dependencies;
    }
  }
}