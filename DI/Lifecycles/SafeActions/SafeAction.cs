using System;

namespace FeaturesDI.Lifecycles.SafeActions
{
  public struct Safe
  {
    public event Action<Exception> Failed;
    
    private readonly Action _action;
    
    public Safe(Action action)
    {
      _action = action;
      Failed = null;
    }

    public void Invoke()
    {
      try
      {
        _action?.Invoke();
      }
      catch (Exception exception)
      {
        Failed?.Invoke(exception);
      }
    }
  }
}