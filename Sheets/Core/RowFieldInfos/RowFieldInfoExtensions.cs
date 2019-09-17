using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sheets.Core.Attributes;
using Sheets.Core.RowFieldInfos.Derived;

namespace Sheets.Core.RowFieldInfos
{
  internal static class RowFieldInfoExtensions
  {
    internal static IEnumerable<RowFieldInfo> GetRowFields(this Type type)
    {
      return type.GetFields()
        .Where(f => f.GetCustomAttribute<RowAttribute>() != null)
        .Select(Create);
    }
    
    private static RowFieldInfo Create(FieldInfo fieldInfo)
    {
      var rowAttribute = fieldInfo.GetCustomAttribute<RowAttribute>();
      if(rowAttribute == null)
        throw new InvalidOperationException("Field must have RowAttribute.");

      if(fieldInfo.FieldType == typeof(string))
        return new SimpleRowFieldInfo(fieldInfo, rowAttribute);
      
      if (typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType))
        return new ListRowFieldInfo(fieldInfo, rowAttribute);
        
      return new ComplexRowFieldInfo(fieldInfo, rowAttribute);
    }
  }
}