using System;
using System.Threading.Tasks;
using DependencyInjection.API.Dependencies;
using DependencyInjection.API.Register;
using DependencyInjection.Core;
using DependencyInjection.Core.Dependencies;
using DependencyInjection.Model;
using DependencyInjection.Model.Dependencies;
using DependencyInjection.Model.Exceptions;

namespace DependencyInjection.API
{
  public class DIContainer : IDependencyInjection
  {
    public event Action<Exception> Failed = delegate { };
    
    private readonly IFeatures _features;
    private readonly IDependants _dependants;
    private readonly IDependencies _dependencies;

    public DIContainer()
    {
      var exceptions = Notifications.With(Failed.Invoke);

      _dependencies = new DomainDependencies();
      _features = new Features(exceptions);
      _dependants = new Dependants(_dependencies, _features, exceptions);

      DI.Initialize(this);
    }
    
    public RegisterDependencyExpression<TDependency> Let<TDependency>() where TDependency : IDependency => 
      new RegisterDependencyExpression<TDependency>(_features);

    void IDependencyInjection.Inject(object instance) => 
      _dependants.Inject(instance);

    void IDependencyInjection.Release(object instance) =>
      _dependants.Release(instance);

    public void Initialize() =>
      _features.Lifecycle.Initialize();

    public void Update() =>
      _features.Lifecycle.Update();

    public Task Pause() => 
      _features.Lifecycle.Pause();

    public void Continue() => 
      _features.Lifecycle.Continue();

    public Task Stop()
    {
      var stopTask = _features.Lifecycle.Terminate();
      if (!stopTask.IsCompleted)
        return stopTask.ContinueWith(x => Clear());

      Clear();
      return stopTask;
    }

    private void Clear()
    {
      DI.Clear();
      
      _dependants.Clear();
      _dependencies.Clear();
      _features.Clear();
    }
  }
}