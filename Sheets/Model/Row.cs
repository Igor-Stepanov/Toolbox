using System.Collections.Generic;
using System.Linq;
using Sheets.Core;

namespace Sheets.Model
{
  public class Row : IRow  // First column - Name, other - Values.
  {
    public int Index { get; }
    public string Name { get; }
    public IList<string> Values { get; }
    
    public IList<object> Raw => new List<object>{Name}.Union(Values.Cast<object>()).ToList();

    private Row(int index, string name, IList<string> values)
    {
      Index = index;
      Name = name;
      Values = values;
    }
    
    public static Row Create(IEnumerable<object> cells, int index)
    {
      string name = null;
      var values = new List<string>();

      foreach (var cell in cells)
      {
        if (name == null)
        {
          name = cell.ToString();
          continue;
        }
        
        values.Add(cell?.ToString() ?? string.Empty);
      }
      return new Row(index, name, values);
    }

    public override string ToString() => Name + " " + string.Join(" ", Values);
  }
}