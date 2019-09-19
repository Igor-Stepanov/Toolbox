using Sheets.Core.RowFieldInfos;

namespace Sheets.Model.Rows.Parse
{
  internal struct ParsedRow
  {
    public readonly Row Row;
    public readonly RowField Field;
    public readonly object Value;

    public ParsedRow(Row row, RowField field, object value) =>
      (Row, Field, Value) =
      (row, field, value);
  }
}