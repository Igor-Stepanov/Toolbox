namespace Sheets.Model.Rows.Parse.Extensions
{
  internal static class ObjectExtensions
  {
    public static void SetFieldFrom(this object self, ParsedRow row) => 
      row.Field.SetValue(self, row.Value);
  }
}