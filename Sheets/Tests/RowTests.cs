using System.Linq;
using NUnit.Framework;
using Sheets.Model;

namespace Sheets.Tests
{
  [TestFixture]
  public class RowTest
  {
    [TestCase("One")]
    [TestCase("One", "Two")]
    public void Row_MustHaveValues(params string [] values)
    {
      var rowValues = values.Select(v => (object)v).ToList();
      
      var row = Row.Create(rowValues, 0);
        
      Assert.AreEqual(row.Name, values.FirstOrDefault());
      Assert.AreEqual(row.Values, values.Skip(1));
    }
    
    [TestCase(null, "Value")]
    [TestCase("Value", null)]
    public void Row_MustHaveNameAndValues_WhenNullsPassed(params string [] values)
    {
      var rowValues = values.Select(v => (object)v).ToList();
      
      var row = Row.Create(rowValues, 0);
      
      Assert.IsTrue(row.Name != null && row.Values != null);
    }
  }
}