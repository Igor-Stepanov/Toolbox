using Common.Matching;
using NUnit.Framework;

namespace Tests.Matching
{
  [TestFixture]
  public class RegisteredFeatureTests
  {
    [Test]
    public void WhenFirstConditionMatches_AndOtherChecksExist_ThenShouldReturnNull()
    {
      // Arrange
      var target = string.Empty;

      // Act
      var result = target.Match<object>(_ => _
       .When<string>(then: x => null)
       .When<object>(then: x => x));

      // Assert
      Assert.That(result, Is.Null);
    }
  }
}