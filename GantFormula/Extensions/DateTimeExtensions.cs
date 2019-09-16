using System;

namespace GantFormula.Extensions
{
  public static class DateTimeExtensions
  {
    public static void Next(this ref DateTime date) =>
      date = date.AddDays(1);
  }
}