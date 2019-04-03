using System;

namespace Common.Matching
{
  public struct IsExpression
  {
    private readonly MatchExpression<bool> _inner;

    public IsExpression(object target) =>
      _inner = new MatchExpression<bool>(target, false);

    private IsExpression(MatchExpression<bool> inner) =>
      _inner = inner;

    public IsExpression Or => this; // For syntax purposes

    public IsExpression Is<T>(Predicate<T> and = null) where T : class =>
      new IsExpression(_inner.When<T>(and, then: _ => true));

    public static implicit operator bool(IsExpression self) =>
      self._inner;
  }

  public static class IsExpressionExtensions
  {
    public static IsExpression Is<T>(this object self, Predicate<T> and = null)
      where T : class =>
      new IsExpression(self).Is<T>(and: and);
  }
}
