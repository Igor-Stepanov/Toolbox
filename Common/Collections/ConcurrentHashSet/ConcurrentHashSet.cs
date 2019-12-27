using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static System.Byte;

namespace Common.Collections.ConcurrentHashSet
{
  public class ConcurrentHashSet<T> : ConcurrentDictionary<T, byte>
  {
    public ConcurrentHashSet(IEqualityComparer<T> comparer) 
      : base(comparer) { }

    public bool Add(T value) =>
      TryAdd(value, MinValue);
    
    public bool Remove(T value) =>
      TryRemove(value, out _);

    public new T[] ToArray() =>
      this.Select(x => x.Key).ToArray();
  }
}