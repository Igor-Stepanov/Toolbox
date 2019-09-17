using System.Collections.Generic;
using Sheets.Core;

namespace Sheets.Model
{
  public class Spreadsheets : ISpreadsheets
  {
    private readonly Dictionary<string, ISpreadsheet> _spreadsheets = new Dictionary<string, ISpreadsheet>();
    private readonly ISheetsService _service;

    public Spreadsheets(ISheetsService service)
    {
      _service = service;
    }

    public ISpreadsheet Spreadsheet(string spreadsheetId)
    {
      if (!_spreadsheets.TryGetValue(spreadsheetId, out var spreadsheet))
        _spreadsheets[spreadsheetId] = spreadsheet = new Spreadsheet(_service, spreadsheetId);

      return spreadsheet;
    }
  }
}