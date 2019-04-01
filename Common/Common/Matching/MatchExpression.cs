using System;
using Common.Extensions;

namespace Common.Matching
{
  public struct MatchExpression<TValue>
  {
    private readonly object _target;
    private readonly TValue _value;
    private bool MatchFound => _target == null;

    public MatchExpression(object target, TValue value)
    {
      _target = target;
      _value = value;
    }

    public MatchExpression<TValue> When<TMatch>(Func<TMatch, TValue> then) where TMatch : class =>
      When(null, then);

    public MatchExpression<TValue> When<TMatch>(Predicate<TMatch> and, Func<TMatch, TValue> then) where TMatch : class
    {
      if (MatchFound)
        return this;

      if (_target is TMatch target && and.NullOrMatches(target))
      {
        return new MatchExpression<TValue>(null, then(target));
      }

      return this;
    }

    // Default value if nothing matched.
    public MatchExpression<TValue> Or(TValue @default) => !MatchFound
      ? new MatchExpression<TValue>(null, @default)
      : this;

    public static implicit operator TValue(MatchExpression<TValue> self) =>
      self._value;
  }

  public static class MatchExpressionExtensions
  {
    public static TValue Match<TValue>(this object self, Func<MatchExpression<TValue>, MatchExpression<TValue>> match)
    {
      var expression = new MatchExpression<TValue>(self, default(TValue));

      return match(expression);
    }

    public static TValue When<TValue>(this object self, Func<MatchExpression<TValue>, MatchExpression<TValue>> match)
    {
      var expression = new MatchExpression<TValue>(self, default(TValue));

      return match(expression);
    }
  }
}
