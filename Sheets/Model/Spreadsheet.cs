using System.Collections.Generic;
using Sheets.Core;

namespace Sheets.Model
{
  public class Spreadsheet : ISpreadsheet
  {
    private readonly string _spreadsheetId;
    private readonly Dictionary<string, ISheet> _sheets = new Dictionary<string, ISheet>();
    
    private readonly ISheetsService _service;
    
    public Spreadsheet(ISheetsService service, string spreadsheetId)
    {
      _service = service;
      _spreadsheetId = spreadsheetId;
    }
    
    public ISheet Sheet(string sheetName)
    {
      if (!_sheets.TryGetValue(sheetName, out var sheetWrapper))
        _sheets[sheetName] = sheetWrapper = new Sheet(_service, _spreadsheetId, sheetName);

      return sheetWrapper;
    }
  }
}