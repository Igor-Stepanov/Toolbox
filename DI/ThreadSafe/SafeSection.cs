using System;
using System.Threading;

namespace DIFeatures.ThreadSafe
{
  public class SafeSection
  {
    public SafeSectionExit Section
    {
      get
      {
        Enter();
        return _exit;
      }
    }
    
    private readonly object _guard;
    private readonly SafeSectionExit _exit;

    public SafeSection()
    {
      _guard = new object();
      _exit = new SafeSectionExit(this);
    }

    private void Enter() =>
      Monitor.Enter(_guard);
    
    private void Exit() => 
      Monitor.Exit(_guard);

    public class SafeSectionExit : IDisposable
    {
      private readonly SafeSection _safeSection;

      public SafeSectionExit(SafeSection safeSection) =>
        _safeSection = safeSection;

      public void Dispose() =>
        _safeSection.Exit();
    }
  }
}