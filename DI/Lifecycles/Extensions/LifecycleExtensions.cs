using System;

namespace DI.Lifecycles.Extensions
{
  public static class LifecycleExtensions
  {
    public static TLifecycle StartWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Start += action;
      return self;
    }
    
    public static TLifecycle PauseWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Pause += action;
      return self;
    }
    
    public static TLifecycle ContinueWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Continue += action;
      return self;
    }
    
    public static TLifecycle StopWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Stop += action;
      return self;
    }
  }
}