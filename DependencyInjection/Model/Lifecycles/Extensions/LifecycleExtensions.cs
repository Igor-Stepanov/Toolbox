using System;
using System.Threading.Tasks;

namespace DependencyInjection.Model.Lifecycles.Extensions
{
  public static class LifecycleExtensions
  {
    public static TLifecycle InitializeWith<TLifecycle>(this TLifecycle self, Action action) where TLifecycle : Lifecycle
    {
      self.Initialize += action;
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
    
    public static TLifecycle TerminateWith<TLifecycle>(this TLifecycle self, Func<Task> task) where TLifecycle : Lifecycle
    {
      self.Terminate += task;
      return self;
    }
  }
}