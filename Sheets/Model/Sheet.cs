using System;
using System.Collections.Generic;
using System.Linq;
using Sheets.Core;
using Sheets.Core.RowFieldInfos;

namespace Sheets.Model
{
  public class Sheet : ISheet
  {
    #region Nested
    
    private struct BoundedRowInfo
    {
      public readonly IRow Value;
      public readonly RowFieldInfo Info;

      public BoundedRowInfo(IRow row, RowFieldInfo info)
      {
        Value = row;
        Info = info;
      }
    }
    
    #endregion
    
    private readonly string _sheetName;
    private readonly string _spreadsheetId;

    private readonly ISheetsService _service;
    public IEnumerable<IRow> Rows => _service.FetchRows(_spreadsheetId, _sheetName);
    
    public Sheet(ISheetsService service, string spreadsheetId, string sheetName)
    {
      _service = service;
      _spreadsheetId = spreadsheetId;
      _sheetName = sheetName;
    }

    public IEnumerable<T> Parse<T>() where T : class, new()
    {
      var fieldInfos = typeof(T).GetRowFields().ToList();
      
      var rows = Rows.ToList();
      
      var boundedRows = rows.Where(row => fieldInfos.SingleOrDefault(info => info.CanParse(row)) != null)
        .Select(row => new BoundedRowInfo(row, fieldInfos.First(f => f.CanParse(row))));
      
      var result = new List<T>();
      
      T current = null;
      foreach (var row in boundedRows)
      {
        if(row.Info.IsId)
          result.Add(current = Activator.CreateInstance<T>());

        row.Info.SetValue(current, rows, row.Value);
      }

      return result;
    }
  }
}