using System;
using System.Threading.Tasks;

namespace DIFeatures.Flow.Extensions
{
  public static class LifecycleExtensions
  {
    public static TLifecycle StartWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Start += action;
      return self;
    }
    
    public static TLifecycle UpdateWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Update += action;
      return self;
    }

    
    public static TLifecycle PauseWith<TLifecycle>(this TLifecycle self, Func<Task> task) where TLifecycle : Lifecycle
    {
      self.Pause += task;
      return self;
    }
    
    public static TLifecycle ContinueWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Continue += action;
      return self;
    }
    
    public static TLifecycle StopWith<TLifecycle>(this TLifecycle self, Func<Task> task) where TLifecycle : Lifecycle
    {
      self.Stop += task;
      return self;
    }
  }
}