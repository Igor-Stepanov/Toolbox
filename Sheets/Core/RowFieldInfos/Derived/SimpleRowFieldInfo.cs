using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sheets.Core.Attributes;

namespace Sheets.Core.RowFieldInfos.Derived
{
  public class SimpleRowFieldInfo : RowFieldInfo
  {
    public SimpleRowFieldInfo(FieldInfo fieldInfo, RowAttribute attribute)
      : base(fieldInfo, attribute)
    {
    }

    public override bool CanParse(IRow row)
    {
      return Attribute.Name == row.Name && row.Values.Count > 0;
    }

    protected override object Parse(IEnumerable<IRow> rows, IRow header)
    {
      return header.Values.First();
    }
  }
}