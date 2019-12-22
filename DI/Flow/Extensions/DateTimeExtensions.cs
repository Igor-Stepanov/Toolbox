using System;

namespace DIFeatures.Flow.Extensions
{
  public static class DateTimeExtensions
  {
    public static TimeSpan Milliseconds(this int self) =>
      TimeSpan.FromMilliseconds(self);
  }
}