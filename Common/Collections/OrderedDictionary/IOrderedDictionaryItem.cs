namespace Common.Collections.OrderedDictionary
{
  public interface IOrderedDictionaryItem<out TKey>
  {
    TKey Key { get; }
  }
}