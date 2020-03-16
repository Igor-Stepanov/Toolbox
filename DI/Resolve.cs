using System.Collections.Generic;

namespace DI.Public
{
  public static class Resolve
  {
    public static T One<T>() where T : class =>
      Container.Resolve<T>();

    public static IEnumerable<T> All<T>() where T : class =>
      Container.ResolveAll<T>();
  }
}