using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Extensions;

namespace Common.Collections.OrderedDictionary
{
  public class OrderedDictionary<TKey, TValue> : IList<TValue>, IList
      where TValue : class, IOrderedDictionaryItem<TKey>
  {
    public int Count => _dictionary.Count;
    public bool IsReadOnly => false;
    
    protected readonly List<TValue> _list;
    protected readonly Dictionary<TKey, TValue> _dictionary;

    public OrderedDictionary()
    {
      var a = new int[3];
      _list = new List<TValue>();
      _dictionary = new Dictionary<TKey, TValue>();
    }

    public OrderedDictionary(IEnumerable<TValue> values) : this() => 
      AddRange(values);

    #region Read

    public TValue this[TKey key] => _dictionary[key];

    public bool Contains(TValue item) => 
      _dictionary.ContainsKey(item.Key);

    public void CopyTo(TValue[] array, int arrayIndex) => 
      _list.CopyTo(array, arrayIndex);

    public int IndexOf(TKey key) => 
      _list.IndexOf(this[key]);

    public int IndexOf(TValue item) => 
      _list.IndexOf(item);

    public List<TValue>.Enumerator GetEnumerator() => 
      _list.GetEnumerator();

    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => 
      GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
      GetEnumerator();

    public ReadOnlyCollection<TValue> AsReadOnly() => 
      _list.AsReadOnly();

    #endregion

    #region Edit

    public void Add(TValue item)
    {
      _dictionary.Add(item.Key, item);
      _list.Add(item);
    }
    
    public void AddRange(IEnumerable<TValue> items) =>
      items.ForEach(Add);

    public void Insert(int index, TValue item)
    {
      _dictionary.Add(item.Key, item);
      _list.Insert(index, item);
    }

    public bool Remove(TValue item)
    {
      var key = item.Key;
      var value = _dictionary[key];

      var removedFromDictionary = _dictionary.Remove(key);
      var removedFromList = _list.Remove(value);

      if (removedFromDictionary ^ removedFromList)
        throw new InvalidOperationException();
      
      return removedFromDictionary;
    }

    public int RemoveAll(Predicate<TValue> predicate)
    {
      var count = 0;
      for (var i = Count - 1; i >= 0; i--)
      {
        if (_list[i].Matches(predicate))
        {
          count++;
          RemoveAt(i);
        }
      }
      return count;
    }

    public void RemoveAt(int index)
    {
      var value = _list[index];

      _dictionary.Remove(value.Key);
      _list.RemoveAt(index);
    }

    public void Clear()
    {
      _dictionary.Clear();
      _list.Clear();
    }

    public TValue this[int index]
    {
      get => _list[index];
      set
      {
        var val = _list[index];
        var key = val.Key;

        _list[index] = value;
        _dictionary[key] = value;
      }
    }

    #endregion

    #region IList

    int IList.Add(object value)
    {
      if (!(value is TValue typedValue))
        throw new ArgumentException("value");
      
      Add(typedValue);
      return Count - 1;
    }

    bool IList.Contains(object value)
    {
      if (!(value is TValue typedValue))
        throw new ArgumentException("value");
      
      return Contains(typedValue);
    }

    int IList.IndexOf(object value)
    {
      if (!(value is TValue typedValue))
        throw new ArgumentException("value");
      
      return IndexOf(typedValue);
    }

    void IList.Insert(int index, object value)
    {
      if (!(value is TValue typedValue))
        throw new ArgumentException("value");
      
      Insert(index, typedValue);
    }

    void IList.Remove(object value)
    {
      if (!(value is TValue typedValue))
        throw new ArgumentException("value");
      
      Remove(typedValue);
    }

    object IList.this[int index]
    {
      get => this[index];
      set
      {
        if (!(value is TValue typedValue))
          throw new ArgumentException("value");
        
        this[index] = typedValue;
      }
    }

    bool IList.IsFixedSize => false;

    void ICollection.CopyTo(Array array, int index) => 
      ((ICollection)_list).CopyTo(array, index);

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => ((ICollection)_list).SyncRoot;

    #endregion
  }
}