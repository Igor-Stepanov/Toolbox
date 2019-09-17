using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Sheets.Core;
using Sheets.Core.Attributes;
using Sheets.Model;

namespace Sheets.Tests
{
  [TestFixture]
  public class SheetTest
  {
    [Test]
    public void ParsedTestObject_MustBeEqualToExpectedResult()
    {
      var service = Substitute.For<ISheetsService>();
      service.FetchRows(Arg.Any<string>(), Arg.Any<string>())
        .Returns(info => TestRows());

      var results = new Spreadsheets(service)
        .Spreadsheet("dummy")
        .Sheet("dummy")
        .Parse<TestObject>();

      var result = results.FirstOrDefault();
      Assert.That(result, Is.Not.Null);
      Assert.That(result.TypeId, Is.EqualTo(ExpectedResult.TypeId));
      Assert.That(result.Items, Is.EqualTo(ExpectedResult.Items));
      Assert.That(result.ComplexItem, Is.EqualTo(ExpectedResult.ComplexItem));
    }
    
    #region Helpers
    
    private static readonly TestObject ExpectedResult = new TestObject
    {
      TypeId = "1",
      ComplexItem = new TestObject.TestItem
      {
        Field1 = "Value1",
        Field2 = "Value2"
      },
      Items = new List<TestObject.TestItem>
      {
        new TestObject.TestItem
        {
          Field1 = "Value1",
          Field2 = "Value2"
        },
        new TestObject.TestItem
        {
          Field1 = "Value1",
          Field2 = "Value2"
        },
      }
    };
    
    public class TestObject : IEquatable<TestObject>
    {
      [IdRow("TypeId")] public string TypeId;
      [Row("ComplexItem")] public TestItem ComplexItem;
      [Row("Items")] public List<TestItem> Items;
      
      public class TestItem : IEquatable<TestItem>
      {
        [Row("Field1")] public string Field1;
        [Row("Field2")] public string Field2;
        
        #region Equals
        
        public bool Equals(TestItem other)
        {
          if (ReferenceEquals(null, other)) return false;
          if (ReferenceEquals(this, other)) return true;
          return string.Equals(Field1, other.Field1) && string.Equals(Field2, other.Field2);
        }

        public override bool Equals(object obj)
        {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != GetType()) return false;
          return Equals((TestItem) obj);
        }
        
        #endregion

        public override string ToString()
        {
          return $"Field1: \"{Field1}\", Field2: \"{Field2}\"";
        }
      }

      #region Equals
      
      public bool Equals(TestObject other)
      {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        var itemsEquals = true;

        if (Items == null ^ other.Items == null)
          itemsEquals = false;
        else if (Items.Count != other.Items.Count)
          itemsEquals = false;
        else
        {
          for (var i = 0; i < Items.Count; i++)
          {
            if (!Items[i].Equals(other.Items[i]))
            {
              itemsEquals = false;
              break;
            }
          }
        }
        
        return string.Equals(TypeId, other.TypeId)
               && Equals(ComplexItem, other.ComplexItem)
               && itemsEquals;
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TestObject) obj);
      }
      
      #endregion
    }

    private static IEnumerable<IRow> TestRows()
    {
      var index = 0;
      
      //TypeId
      var typeIdRow = Substitute.For<IRow>();
      typeIdRow.Index.Returns(index++);
      typeIdRow.Name.Returns(_ => "TypeId");
      typeIdRow.Values.Returns(_ => new List<string>{"1"});
      yield return typeIdRow;
      
      
      // Complex item
      var complexItemHeaderRow = Substitute.For<IRow>();
      complexItemHeaderRow.Index.Returns(index++);
      complexItemHeaderRow.Name.Returns(_ => "ComplexItem");
      complexItemHeaderRow.Values.Returns(_ => new List<string>{"Field1", "Field2"});
      yield return complexItemHeaderRow;
      
      var complexItemValueRow = Substitute.For<IRow>();
      complexItemValueRow.Index.Returns(index++);
      complexItemValueRow.Name.Returns(_ => string.Empty);
      complexItemValueRow.Values.Returns(_ => new List<string>{"Value1", "Value2"});
      yield return complexItemValueRow;
      
      // Items list
      var itemsListHeaderRow = Substitute.For<IRow>();
      itemsListHeaderRow.Index.Returns(index++);
      itemsListHeaderRow.Name.Returns(_ => "Items");
      itemsListHeaderRow.Values.Returns(_ => new List<string>{"Field1", "Field2"});
      yield return itemsListHeaderRow;
      
      var listItemValueRow1 = Substitute.For<IRow>();
      listItemValueRow1.Index.Returns(index++);
      listItemValueRow1.Name.Returns(_ => string.Empty);
      listItemValueRow1.Values.Returns(_ => new List<string>{"Value1", "Value2"});
      yield return listItemValueRow1;
      
      var emptyRow = Substitute.For<IRow>();
      emptyRow.Index.Returns(index++);
      emptyRow.Name.Returns(string.Empty);
      emptyRow.Values.Returns(new List<string>());
      yield return emptyRow;
      
      var listItemValueRow2 = Substitute.For<IRow>();
      listItemValueRow2.Index.Returns(index++);
      listItemValueRow2.Name.Returns(_ => string.Empty);
      listItemValueRow2.Values.Returns(_ => new List<string>{"Value1", "Value2"});
      yield return listItemValueRow2;
    }
    
    #endregion
  }
}