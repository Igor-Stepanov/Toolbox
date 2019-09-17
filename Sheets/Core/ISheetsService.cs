using System.Collections.Generic;

namespace Sheets.Core
{
  public interface ISheetsService
  {
    IEnumerable<IRow> FetchRows(string spreadsheetId, string sheetName);
  }
}