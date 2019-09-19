using System.Collections.Generic;
using Google.Apis.Sheets.v4;
using Sheets.Core;

namespace Sheets.Model
{
  public class Spreadsheets : ISpreadsheets
  {
    private readonly SheetsService _service;
    private readonly Dictionary<string, ISpreadsheet> _spreadsheets;

    internal Spreadsheets(SheetsService service)
    {
      _service = service;
      _spreadsheets = new Dictionary<string, ISpreadsheet>();
    }

    public ISpreadsheet Spreadsheet(string name)
    {
      if (!_spreadsheets.TryGetValue(name, out var spreadsheet))
        _spreadsheets[name] = spreadsheet = new Spreadsheet(_service, name);

      return spreadsheet;
    }
  }
}