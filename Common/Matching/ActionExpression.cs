using System;
using Common.Extensions;

namespace Common.Matching
{
  public struct ActionExpression
  {
    private readonly object _target;
    private bool MatchFound => _target == null;

    public ActionExpression(object target) =>
      _target = target;

    public ActionExpression When<TMatch>(Action<TMatch> then) where TMatch : class =>
      When(null, then);

    public ActionExpression When<TMatch>(Predicate<TMatch> and, Action<TMatch> then) where TMatch : class
    {
      if (MatchFound)
        return this;

      if (_target is TMatch target && and.NullOrMatches(target))
      {
        then(target);
        return new ActionExpression(null);
      }

      return this;
    }

    public void Or(Action<object> @default) // Default value if nothing matched.
    {
      if (_target != null)
        @default(_target);
    }
  }

  public static class MatchActionExpressionExtensions
  {
    public static ActionExpression MatchAction(this object self, Func<ActionExpression, ActionExpression> match)
    {
      return match(new ActionExpression(self));
    }
  }
}
