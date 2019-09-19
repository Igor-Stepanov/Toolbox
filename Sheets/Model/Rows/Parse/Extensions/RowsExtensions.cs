using System.Collections.Generic;
using System.Linq;
using Sheets.Core.RowFieldInfos;

namespace Sheets.Model.Rows.Parse.Extensions
{
  internal static class RowsExtensions
  {
    public static IEnumerable<ParsedRow> ParsedAs(this List<Row> self, RowField[] fields)
    {
      foreach (var row in self)
      {
        var field = fields.Single(x => x.CanParse(row));
        var value = field.Parse(self, row);
        
        yield return new ParsedRow(row, field, value);
      }
    }
  }
}