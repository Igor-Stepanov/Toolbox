using System.Collections.Generic;
using Sheets.Core.Cells;

namespace Sheets.Core
{
  public interface ISheet
  {
    IReadOnlyList<IRow> Rows { get; }
    
    IEnumerable<T> Parse<T>() where T : class, new();
    
    Cell this[int x, int y] { get; set; } // Zero-based

    void Save();
  }
}