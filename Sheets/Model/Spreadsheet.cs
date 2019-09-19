using System.Collections.Generic;
using Google.Apis.Sheets.v4;
using Sheets.Core;

namespace Sheets.Model
{
  public class Spreadsheet : ISpreadsheet
  {
    private readonly string _spreadsheetId;
    private readonly Dictionary<string, ISheet> _sheets = new Dictionary<string, ISheet>();
    
    private readonly SheetsService _service;
    
    public Spreadsheet(SheetsService service, string spreadsheetId)
    {
      _service = service;
      _spreadsheetId = spreadsheetId;
    }
    
    public ISheet Sheet(string name)
    {
      if (!_sheets.TryGetValue(name, out var sheet))
        _sheets[name] = sheet = new Sheet(_service, _spreadsheetId, name);

      return sheet;
    }
  }
}