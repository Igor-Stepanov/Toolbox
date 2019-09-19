using System.Collections.Generic;
using Sheets.Core.Cells;

namespace Sheets.Core
{
  public interface IRow
  {
    int Index { get; }
    IReadOnlyList<Cell> Cells { get; }
    Cell this[int index] { get; set; }
    
    List<Cell>.Enumerator GetEnumerator();
  }
}