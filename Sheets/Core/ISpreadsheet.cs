namespace Sheets.Core
{
  public interface ISpreadsheet
  {
    ISheet Sheet(string sheetName);
  }
}