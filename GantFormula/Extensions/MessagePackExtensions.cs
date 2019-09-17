using MessagePack;

namespace GantFormula.Extensions
{
  public static class MessagePackExtensions
  {
    public static T Copy<T>(this T self)
    {
      var serialized = MessagePackSerializer.Serialize(self);
      return MessagePackSerializer.Deserialize<T>(serialized);
    }
  }
}