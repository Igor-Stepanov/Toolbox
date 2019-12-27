using System;
using DependencyInjection.Core.Exceptions;

namespace DependencyInjection.Model.Exceptions
{
  public class Notifications : IExceptions
  {
    public event Action<Exception> ExceptionOccured;

    private Notifications() { }

    public void Handle(Exception exception) =>
      ExceptionOccured?.Invoke(exception);
    
    public static Notifications With(Action<Exception> callback)
    {
      var notifications = new Notifications();
      notifications.ExceptionOccured += callback;
      return notifications;
    }
  }
}