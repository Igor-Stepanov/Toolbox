using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Extensions;
using Common.Reflection.Extensions;

namespace Common.Reflection
{
  public static class Types
  {
    public struct AssemblyExpression
    {
      private readonly Predicate<Type> _predicate;

      public AssemblyExpression(Predicate<Type> predicate) =>
        _predicate = predicate;

      public IEnumerable<Type> InThisAssembly() => In(Assembly.GetCallingAssembly());
      public IEnumerable<Type> In(Assembly assembly)
      {
        var predicate = _predicate;
        return assembly.Types()
          .Where(x => x.Matches(predicate));
      }

      public IEnumerable<Type> InCurrentDomain() => In(AppDomain.CurrentDomain);
      public IEnumerable<Type> In(AppDomain domain)
      {
        var predicate = _predicate;
        return domain.GetAssemblies()
          .SelectMany(x => x.Types())
          .Where(x => x.Matches(predicate));
      }
    }

    public static AssemblyExpression Of<T>() where T : class =>
      new AssemblyExpression(x => typeof(T).IsInterface && x.Implements<T>()
                               || typeof(T).IsClass && x.DerivedFrom<T>());

    public static AssemblyExpression Implementing<T>() where T : class =>
      new AssemblyExpression(x => typeof(T).IsInterface && x.Implements<T>());

    public static AssemblyExpression DerivedFrom<T>() where T : class =>
      new AssemblyExpression(x => typeof(T).IsClass && x.DerivedFrom<T>());
    
    public static AssemblyExpression WithAnyFieldWith<TAttribute>(BindingFlags flags) where TAttribute : Attribute =>
      new AssemblyExpression(x => x.FieldsWith<TAttribute>(flags).Any());
  }
}