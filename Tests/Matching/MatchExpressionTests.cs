using System;
using Common.Matching;
using NUnit.Framework;

namespace Tests.Matching
{
  [TestFixture]
  public class MatchExpressionTests
  {
    [Test]
    public void WhenFirstConditionMatches_AndOtherChecksExist_ThenShouldReturnNull()
    {
      // Arrange
      var target = string.Empty;

      // Act
      var result = target.Match<object>(_=>_
            .When<string>(then: x => null)
            .When<object>(then: x => x));

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test]
    public void WhenSecondConditionMatches_AndFirstIsWrong_ThenShouldReturnNull()
    {
      // Arrange
      var target = string.Empty;

      // Act
      var result = target.Match<object>(_ => _
            .When<Exception>(then: x => x)
            .When<string>(then: x => null));

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test]
    public void WhenNothingMatches_AndDefaultValueIsSet_ThenShouldReturnDefault()
    {
      // Arrange
      var target = new object();

      // Act
      var result = target.Match<object>(_ => _
            .When<Exception>(then: x => null)
            .When<string>(then: x => null)
            .Or(target));

      // Assert
      Assert.That(result, Is.EqualTo(target));
    }

    [Test]
    public void WhenTypeMatches_AndAdditionalConditionDont_ThenShouldReturnNull()
    {
      // Arrange
      var target = "kek";

      // Act
      var result = target.Match<object>(_ => _
            .When<string>(and: x => x == "not kek", then: x => x));

      // Assert
      Assert.That(result, Is.Null);
    }
  }
}
