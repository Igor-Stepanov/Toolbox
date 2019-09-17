using System.Collections.Generic;
using System.Reflection;
using Sheets.Core.Attributes;

namespace Sheets.Core.RowFieldInfos
{
  public abstract class RowFieldInfo
  {
    public readonly FieldInfo FieldInfo;
    public readonly RowAttribute Attribute;

    public bool IsId => Attribute is IdRowAttribute;
    
    protected RowFieldInfo(FieldInfo fieldInfo, RowAttribute attribute)
    {
      FieldInfo = fieldInfo;
      Attribute = attribute;
    }

    public void SetValue(object obj, IEnumerable<IRow> rows, IRow header)
    {
      var fieldValue = Parse(rows, header);
      FieldInfo.SetValue(obj, fieldValue);
    }

    public abstract bool CanParse(IRow row);
    protected abstract object Parse(IEnumerable<IRow> rows, IRow header);
  }
}