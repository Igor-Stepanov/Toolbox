using System;

namespace Sheets.Core.Attributes
{
  [AttributeUsage(AttributeTargets.Field)]
  public class IdRowAttribute : RowAttribute
  {
    public IdRowAttribute(string name) : base(name)
    {
    }
  }
}