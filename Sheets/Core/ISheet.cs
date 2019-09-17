using System.Collections.Generic;

namespace Sheets.Core
{
  public interface ISheet
  {
    IEnumerable<T> Parse<T>() where T : class, new();
    IEnumerable<IRow> Rows { get; }
  }
}