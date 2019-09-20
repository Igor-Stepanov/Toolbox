using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Sheets.Core;
using Sheets.Core.Cells;
using Sheets.Core.RowFields.Extensions;
using Sheets.Model.Rows;
using Sheets.Model.Rows.Parse.Extensions;
using static System.Activator;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum;
using static Sheets.Core.Cells.Cell;
using static Sheets.Model.Rows.Row;

namespace Sheets.Model
{
  public class Sheet : ISheet
  {
    public IReadOnlyList<IRow> Rows => _rows;

    private readonly string _sheetName;
    private readonly string _spreadsheetName;

    private readonly SheetsService _service;
    private readonly List<Row> _rows;
    
    public Sheet(SheetsService service, string spreadsheetName, string sheetName)
    {
      _service = service;
      _spreadsheetName = spreadsheetName;
      _sheetName = sheetName;
      
      _rows = new List<Row>(_service
       .Spreadsheets
       .Values
       .Get(_spreadsheetName, _sheetName)
       .Execute()
       .Values
       .Select(AsRow));
    }

    public IEnumerable<T> Parse<T>() where T : class, new()
    {
      var fields = typeof(T).RowFields().ToArray();
      var parsedRows = _rows.ParsedAs(fields);
      
      var result = new List<T>();
      
      T current = null;
      
      foreach (var parsedRow in parsedRows)
      {
        if (parsedRow.Field.IsId) 
          result.Add(current = CreateInstance<T>());

        current.SetFieldFrom(parsedRow);
      }

      return result;
    }

    public Cell this[int x, int y]
    {
      get => _rows.Count <= y ? Empty : _rows[y][x];
      set
      {
        if (_rows.Count < y)
        {
          for (var i = _rows.Count; i < y; i++)
            _rows.Add(AsRow(new List<object>(), i));
        }

        _rows[y][x] = value;
      }
    }

    public void Save()
    {
      var valueRange = new ValueRange
      {
        Values = _rows
         .Select(x => x.Raw)
         .ToList()
      };
      
      var updateRequest = _service.Spreadsheets.Values.Update(valueRange, _spreadsheetName, _sheetName);
      updateRequest.ValueInputOption = RAW;
      updateRequest.Execute();
    }
  }
}