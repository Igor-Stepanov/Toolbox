namespace Sheets.Core.Cells
{
  public struct Cell
  {
    public object Value;

    public static implicit operator string(Cell self) =>
      self.Value?.ToString();

    public static implicit operator Cell(string self) =>
      new Cell {Value = self};
    
    public static implicit operator Cell(int self) =>
      new Cell {Value = self.ToString()};
    
    public static Cell Empty => new Cell();
    public static Cell AsCell(object value) =>
      new Cell { Value = value };
  }
}