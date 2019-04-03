namespace Common.Extensions
{
  public static class ObjectExtensions
  {
    public static T As<T>(this object self) where T : class
    {
      return self as T;
    }
  }
}
