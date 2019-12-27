using System;
using System.Threading.Tasks;
using DependencyInjection.Core.Lifecycles;
using DependencyInjection.Model.Exceptions;
using DependencyInjection.Model.Lifecycles;
using DependencyInjection.Model.Lifecycles.Extensions;
using static System.Threading.Tasks.Task;

namespace DependencyInjection.API.Dependencies
{
  public abstract class Feature : IDependency
  {
    internal event Action<Exception> Failed;
    internal ILifecycle Lifecycle { get; }

    protected Feature()
    {
      var exceptions = Notifications
       .With(x => Failed?.Invoke(x.AsFeatureException(this)));
      
      Lifecycle = new Lifecycle(exceptions)
       .InitializeWith(Initialize)
       .UpdateWith(Update);
    }

    protected virtual void Initialize() { }
    protected virtual void Update() { }
    protected virtual Task Pause() => CompletedTask;
    protected virtual void Continue() { }
    protected virtual Task Terminate() => CompletedTask;

    public override string ToString() =>
      $"{GetType().Name}: {Lifecycle}";
  }
}