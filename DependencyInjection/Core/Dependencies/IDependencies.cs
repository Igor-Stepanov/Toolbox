using DependencyInjection.Model.Dependencies;

namespace DependencyInjection.Core.Dependencies
{
  internal interface IDependencies
  {
    InstanceDependencies Of(object instance);
    void Clear();
  }
}