using System;
using System.Threading;

namespace Common.Multithreading.ThreadSafe
{
  public class ThreadSafeSection
  {
    public ExitOnDispose Section
    {
      get
      {
        Enter();
        return _exit;
      }
    }
    
    private readonly object _guard;
    private readonly ExitOnDispose _exit;

    public ThreadSafeSection()
    {
      _guard = new object();
      _exit = new ExitOnDispose(this);
    }

    private void Enter() =>
      Monitor.Enter(_guard);
    
    private void Exit() => 
      Monitor.Exit(_guard);

    public class ExitOnDispose : IDisposable
    {
      private readonly ThreadSafeSection _safeSection;

      public ExitOnDispose(ThreadSafeSection safeSection) =>
        _safeSection = safeSection;

      public void Dispose() =>
        _safeSection.Exit();
    }
  }
}