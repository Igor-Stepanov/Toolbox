using System;

namespace Sheets.Core.Attributes
{
  [AttributeUsage(AttributeTargets.Field)]
  public class RowAttribute : Attribute
  {
    public readonly string Name;

    public RowAttribute(string name)
    {
      Name = name;
    }
  }
}