using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Extensions;
using Common.Reflection.Extensions;

namespace Common.Reflection
{
  public static class Instances
  {
    public struct AssemblyExpression<T>
    {
      private readonly Predicate<Type> _predicate;

      public AssemblyExpression(Predicate<Type> predicate) =>
        _predicate = predicate;

      public List<T> InThisAssembly() => In(Assembly.GetCallingAssembly());
      public List<T> In(Assembly assembly)
      {
        var predicate = _predicate;

        return assembly.GetTypes()
                       .Where(x => x.Matches(predicate))
                       .Select(x => x.NewInstance())
                       .Cast<T>()
                       .ToList();
      }

      public List<T> InThisDomain() => In(AppDomain.CurrentDomain);
      public List<T> In(AppDomain domain)
      {
        var predicate = _predicate;
        return domain.GetAssemblies()
                     .SelectMany(x => x.GetTypes())
                     .Where(x => x.Matches(predicate))
                     .Select(x => x.NewInstance())
                     .Cast<T>()
                     .ToList();
      }
    }

    public static AssemblyExpression<T> Of<T>() where T : class =>
      new AssemblyExpression<T>(x => typeof(T).IsInterface && x.Implements<T>() || typeof(T).IsClass && x.DerivedFrom<T>());

    public static AssemblyExpression<T> Implementing<T>() where T : class =>
      new AssemblyExpression<T>(x => typeof(T).IsInterface && x.Implements<T>());

    public static AssemblyExpression<T> DerivedFrom<T>() where T : class =>
      new AssemblyExpression<T>(x => typeof(T).IsClass && x.DerivedFrom<T>());
  }
}
