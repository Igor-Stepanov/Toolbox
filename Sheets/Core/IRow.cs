using System.Collections.Generic;

namespace Sheets.Core
{
  public interface IRow
  {
    int Index { get; }
    string Name { get; }
    IList<string> Values { get; }

    IList<object> Raw { get; }
  }
}