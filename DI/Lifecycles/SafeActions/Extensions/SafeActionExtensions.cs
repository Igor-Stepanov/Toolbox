using System;

namespace FeaturesDI.Lifecycles.SafeActions.Extensions
{
  public static class SafeActionExtensions
  {
    public static Safe WhenFailed(this Safe self, Action<Exception> handle)
    {
      self.Failed += handle;
      return self;
    }
  }
}