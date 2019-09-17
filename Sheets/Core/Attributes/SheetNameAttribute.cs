using System;

namespace Sheets.Core.Attributes
{
  /// <summary>
  ///   Represents requirement to sheet name, which data-class can be parsed from.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class SheetNameAttribute : Attribute
  {
    public SheetNameAttribute(string name)
    {
      Name = name;
    }
    
    public string Name { get; }
  }
}