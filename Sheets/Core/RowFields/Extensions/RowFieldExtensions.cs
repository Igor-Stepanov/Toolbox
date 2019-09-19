using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Reflection.Extensions;
using Sheets.Core.Attributes;
using Sheets.Core.RowFieldInfos;

namespace Sheets.Core.RowFields.Extensions
{
  internal static class RowFieldExtensions
  {
    internal static IEnumerable<RowField> RowFields(this Type type) => type
     .GetFields()
     .Where(x => FieldInfoExtensions.Has<RowAttribute>(x))
     .Select(RowField);    

    private static RowField RowField(FieldInfo fieldInfo)
    {
      var rowAttribute = fieldInfo.GetCustomAttribute<RowAttribute>();
      if (rowAttribute == null)
        throw new InvalidOperationException("Field must have RowAttribute.");

      if (fieldInfo.FieldType == typeof(string))
        return new RowField(fieldInfo, rowAttribute);

      if (typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType))
        return new ListRowField(fieldInfo, rowAttribute);

      return new ComplexRowField(fieldInfo, rowAttribute);
    }
  }
}