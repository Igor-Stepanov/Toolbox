using System.Collections.Generic;

namespace Common.Hashes
{
  public struct HashExpression
  {
    private int _hash;
    
    public HashExpression With<T>(T o)
    {
      Append(Hash(o));
      return this;
    }

    public HashExpression With<T>(T[] data)
    {
      if (data == null)
        return this;

      var count = data.Length;
      for (var i = 0; i < count; ++i)
        Append(Hash(data[i]));

      return this;
    }

    public HashExpression With<T>(List<T> data)
    {
      if (data == null)
        return this;

      foreach (var item in data)
        Append(Hash(item));

      return this;
    }

    public HashExpression With<T>(IEnumerable<T> data)
    {
      if (data == null)
        return this;

      foreach (var item in data)
        Append(Hash(item));

      return this;
    }

    public HashExpression With<TKey, TValue>(Dictionary<TKey, TValue> data)
    {
      if (data == null)
        return this;

      foreach (var pair in data)
      {
        var key = Hash(pair.Key);
        var value = Hash(pair.Value);

        Append(unchecked ((key * 397) ^ value));
      }

      return this;
    }

    private void Append(int hash) => _hash = unchecked ((_hash * 397) ^ hash);
    
    private static int Hash<T>(T o) => 
      EqualityComparer<T>.Default.GetHashCode(o);

    public static implicit operator int(HashExpression self) =>
      self._hash;
  }
}