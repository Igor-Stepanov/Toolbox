using System.Collections.Generic;

namespace Common.Hashes
{
  public static class Hash
  {
    public static HashExpression Of<T>(T o) => new HashExpression().With(o);
    public static HashExpression Of<T>(T[] o) => new HashExpression().With(o);
    public static HashExpression Of<T>(List<T> o) => new HashExpression().With(o);
    public static HashExpression Of<T>(IEnumerable<T> o) => new HashExpression().With(o);
    public static HashExpression Of<TKey, TValue>(Dictionary<TKey, TValue> o) => new HashExpression().With(o);
  }
}