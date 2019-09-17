using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sheets.Core.Attributes;

namespace Sheets.Core.RowFieldInfos.Derived
{
  public class ComplexRowFieldInfo : RowFieldInfo
  {
    public ComplexRowFieldInfo(FieldInfo fieldInfo, RowAttribute attribute)
      : base(fieldInfo, attribute)
    {
    }

    public override bool CanParse(IRow row)
    {
      if (Attribute.Name != row.Name)
        return false;

      var rowFields = FieldInfo.FieldType.GetRowFields();
      
      return row.Values.Any(value => rowFields.SingleOrDefault(info => info.Attribute.Name == value) != null);
    }
    
    protected override object Parse(IEnumerable<IRow> rows, IRow header)
    {
      var fieldByValueIndex = FieldInfo.FieldType.GetRowFields()
        .Where(x => header.Values.SingleOrDefault(cellName => cellName == x.Attribute.Name) != null)
        .ToDictionary(x => header.Values.IndexOf(x.Attribute.Name), x => x);

      var instance = Activator.CreateInstance(FieldInfo.FieldType);

      var fieldValuesRow = rows.First(row => row.Index == header.Index + 1);

      for (var i = 0; i < fieldValuesRow.Values.Count; i++)
      {
        if (fieldByValueIndex.TryGetValue(i, out var rowFieldInfo))
        {
          rowFieldInfo.FieldInfo.SetValue(instance, fieldValuesRow.Values[i]);
        }
      }

      return instance;
    }
  }
}