using System;
// ReSharper disable All

namespace Sandbox.UDev.Wrong
{
  public class BadBottle
  {
    private Action _onOpened;

    public BadBottle(Action onOpened) =>
      _onOpened = onOpened;

    public void Open()
    {
      // ...
      _onOpened?.Invoke();
    }
  }
}