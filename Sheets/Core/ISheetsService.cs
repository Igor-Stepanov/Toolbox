using System.Collections.Generic;

namespace Sheets.Core
{
  public interface ISheetsService
  {
    IEnumerable<IRow> FetchRows(string spreadsheetId, string sheetName);
    void Update(string spreadsheetId, string sheetName, IEnumerable<IRow> rows);
  }
}