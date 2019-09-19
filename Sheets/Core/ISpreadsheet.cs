namespace Sheets.Core
{
  public interface ISpreadsheet
  {
    ISheet Sheet(string name);
  }
}