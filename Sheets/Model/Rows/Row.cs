using System.Collections.Generic;
using System.Linq;
using Sheets.Core;
using Sheets.Core.Cells;
using static System.Linq.Enumerable;
using static System.String;
using static Sheets.Core.Cells.Cell;

namespace Sheets.Model.Rows
{
  public class Row : IRow  // First column - Name, other - Values.
  {
    public int Index { get; }
    public IReadOnlyList<Cell> Cells => _cells;

    public IList<object> Raw =>
      _cells.Cast<object>().ToList();

    private readonly List<Cell> _cells;

    private Row(int index, IEnumerable<object> cells)
    {
      Index = index;
      _cells = cells.Select(AsCell).ToList();
    }    

    public Cell this[int index]
    {
      get => _cells.Count <= index ? Cell.Empty : _cells[index];
      set
      {
        if (_cells.Count < index)
        {
          for (var i = _cells.Count; i < index; i++)
            _cells.Add(Cell.Empty);
        }

        _cells[index] = value;
      }
    }

    public List<Cell>.Enumerator GetEnumerator() =>
      _cells.GetEnumerator();

    public override string ToString() =>
       Join(" ", _cells.Select((x,i) => $"{i}:[{x}]"));
    
    public static Row New(int index) => 
      new Row(index, Empty<object>());

    public static Row AsRow(IList<object> cells, int index) => 
      new Row(index, cells);
  }
}