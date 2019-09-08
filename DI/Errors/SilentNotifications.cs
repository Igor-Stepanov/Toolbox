using System;

namespace DIFeatures.Errors
{
  public class SilentNotifications : IErrors
  {
    private readonly Action<Exception> _notify;

    public SilentNotifications(Action<Exception> with) =>
      _notify = with;

    public void Handle(Exception exception) =>
      _notify(exception);
  }
}