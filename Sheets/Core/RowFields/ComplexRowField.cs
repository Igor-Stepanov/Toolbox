using System.Collections.Generic;
using System.Reflection;
using Common.Extensions;
using Sheets.Core.Attributes;
using Sheets.Core.RowFieldInfos;
using Sheets.Core.RowFields.Extensions;
using static System.Activator;

namespace Sheets.Core.RowFields
{
  public class ComplexRowField : RowField
  {
    private IEnumerable<RowField> Fields => Field.FieldType.RowFields();

    public ComplexRowField(FieldInfo field, RowAttribute attribute) : base(field, attribute)
    {
    }
    
    public override object Parse(IReadOnlyList<IRow> rows, IRow row)
    {
      var instance = CreateInstance(Field.FieldType);
      
      foreach (var field in Fields)
      {
        var index = row.Cells.IndexOf(field.Name);
        if (index == null)
          continue;

        field.SetValue(instance, rows[row.Index + 1][index.Value]);
      }

      return instance;
    }
  }
}