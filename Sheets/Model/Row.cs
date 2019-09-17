using System.Collections.Generic;
using Sheets.Core;

namespace Sheets.Model
{
  public class Row : IRow  // First column - Name, other - Values.
  {
    public int Index { get; }
    public string Name { get; }
    public IList<string> Values { get; }
    
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

      foreach (string cell in cells)
      {
        if (name == null)
        {
          name = cell;
          continue;
        }
        
        values.Add(cell ?? string.Empty);
      }
      return new Row(index, name, values);
    }
  }
}