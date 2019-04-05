namespace Common.Extensions
{
  public static class NullableExtensions
  {
    public static T Or<T>(this T? self, T value) where T : struct =>
      self ?? value; 
    
    public static T OrDefault<T>(this T? self) where T : struct =>
      self.GetValueOrDefault();
  }
}
