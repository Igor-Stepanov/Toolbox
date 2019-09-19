using System.Collections.Generic;
using System.Reflection;
using Sheets.Core.Attributes;

namespace Sheets.Core.RowFieldInfos
{
  public class RowField
  {
    protected readonly RowAttribute Attribute;
    protected readonly FieldInfo Field;

    public bool IsId => Attribute is IdRowAttribute;
    public string Name => Attribute.Name;
    
    public RowField(FieldInfo field, RowAttribute attribute)
    {
      Field = field;
      Attribute = attribute;
    }

    public void SetValue(object instance, object value) => 
      Field.SetValue(instance, value);

    public virtual bool CanParse(IRow row) => Attribute.Name == row[0];
    public virtual object Parse(IReadOnlyList<IRow> rows, IRow row) => row[1];
  }
}