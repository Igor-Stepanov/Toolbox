using System;
using System.Threading.Tasks;
using DependencyInjection.Core.Exceptions;
using DependencyInjection.Core.Lifecycles;
using DependencyInjection.Model.Lifecycles.Extensions;
using static System.Threading.Tasks.Task;
using static DependencyInjection.Model.Lifecycles.State;

namespace DependencyInjection.Model.Lifecycles
{
  public class Lifecycle : ILifecycle
  {
    public event Action Initialize;
    public event Action Update;
    public event Func<Task> Pause;
    public event Action Continue;
    public event Func<Task> Terminate;
    
    private State _state;
    
    private readonly IExceptions _exceptions;

    internal Lifecycle(IExceptions exceptions)
    {
      _exceptions = exceptions;
      _state.To(Created);
    }

    void ILifecycle.Initialize()
    {
      _state.To(Initializing);
      _exceptions.Try(Initialize);
      _state.To(Initialized);
    }
    
    void ILifecycle.Update() =>
      _exceptions.Try(Update);

    Task ILifecycle.Pause()
    {
      _state.To(Pausing);
      var task = _exceptions.Try(Pause) ?? CompletedTask;
      
      if (task.IsCompleted)
        _state.To(Paused);
      else
        task.ContinueWith(x => _state.To(Paused));

      return task;
    }

    void ILifecycle.Continue()
    {
      _state.To(Continuing);
      _exceptions.Try(Continue);
      _state.To(Continued);
    }

    Task ILifecycle.Terminate()
    {
      _state.To(Terminating);
      var task = _exceptions.Try(Terminate) ?? CompletedTask;
      
      if (task.IsCompleted)
        _state.To(Terminated);
      else
        task.ContinueWith(x => _state.To(Terminated));

      return task;
    }

    public override string ToString() =>
      $"{nameof(Lifecycle)}: {_state}";
  }
}