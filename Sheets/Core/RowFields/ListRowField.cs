using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Extensions;
using Sheets.Core.Attributes;
using Sheets.Core.RowFieldInfos;
using Sheets.Core.RowFields.Extensions;
using static System.Activator;
using static Sheets.Core.Cells.Cell;

namespace Sheets.Core.RowFields
{
  public class ListRowField : RowField
  {
    private IEnumerable<RowField> ItemFields => Field
     .FieldType
     .GetGenericArguments()
     .First()
     .RowFields();
    
    public ListRowField(FieldInfo field, RowAttribute attribute)
      : base(field, attribute)
    {
    }

    public override object Parse(IReadOnlyList<IRow> rows, IRow row)
    {
      var list = (IList) CreateInstance(Field.FieldType);
      var listItemType = Field.FieldType.GetGenericArguments().First();

      var itemRows = rows
       .Where(x => x.Index > row.Index)
       .TakeWhile(x => x[0] != Empty);
      
      foreach (var itemRow in itemRows)
      {
        var instance = CreateInstance(listItemType);

        foreach (var field in ItemFields)
        {
          var index = row.Cells.IndexOf(field.Name);
          if (index == null)
            continue;

          field.SetValue(instance, itemRow[index.Value]);
        }

        list.Add(instance);
      }

      return  list;
    }
  }
}