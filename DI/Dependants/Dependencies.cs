using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DI.Dependants
{

  public class Fields
  {
    public IEnumerable<FieldInfo> Fields => _fields;
    private readonly List<FieldInfo> _fields = new List<FieldInfo>();
    
  }
  public static class Dependencies
  {
    private static readonly Dictionary<Type, Fields> _typeDependencies;
    
    static Dependencies()
    {
      _typeDependencies = new Dictionary<Type, Fields>();

      foreach (var VARIABLE in typeof(Dependant).)
      {
        
      }
      
      var current = typeof(Dependencies);
      while (current != null)
      {
        current
          .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
          .SingleOrDefault(x => x.Has<DependenciesAttribute>())
          ?.Invoke(this, parameters);

        current = current.BaseType;
      
    }
  }
}