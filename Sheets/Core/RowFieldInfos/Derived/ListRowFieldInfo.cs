using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sheets.Core.Attributes;

namespace Sheets.Core.RowFieldInfos.Derived
{
  public class ListRowFieldInfo : RowFieldInfo
  {
    public ListRowFieldInfo(FieldInfo fieldInfo, RowAttribute attribute)
      : base(fieldInfo, attribute)
    {
    }

    public override bool CanParse(IRow row)
    {
      if (Attribute.Name != row.Name)
        return false;

      var genericArguments = FieldInfo.FieldType.GenericTypeArguments;
      if (genericArguments == null)
        return false;

      var genericItemType = genericArguments.FirstOrDefault();
      if (genericItemType == null)
        return false;

      var itemRowFields = genericItemType.GetRowFields();
      
      return row.Values.Any(value => itemRowFields.SingleOrDefault(info => info.Attribute.Name == value) != null);
    }

    protected override object Parse(IEnumerable<IRow> rows, IRow header)
    {
      var list = (IList) Activator.CreateInstance(FieldInfo.FieldType);
      var listItemType = FieldInfo.FieldType.GetGenericArguments().First();

      var fieldByKey = listItemType.GetRowFields()
        .Where(x => header.Values.SingleOrDefault(cellName => cellName == x.Attribute.Name) != null)
        .ToDictionary(x => header.Values.IndexOf(x.Attribute.Name), x => x);

      var itemRows = rows.Where(r => r.Index > header.Index && r.Values.Any())
        .TakeWhile(r => string.IsNullOrEmpty(r.Name));
      
      foreach (var itemRow in itemRows)
      {
        var current = Activator.CreateInstance(listItemType);

        for (var i = 0; i < itemRow.Values.Count; i++)
        {
          if (fieldByKey.TryGetValue(i, out var rowFieldInfo))
          {
            rowFieldInfo.FieldInfo.SetValue(current, itemRow.Values[i]);
          }
        }

        list.Add(current);
      }

      return  list;
    }
  }
}