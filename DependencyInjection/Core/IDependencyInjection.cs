namespace DependencyInjection.Core
{
  internal interface IDependencyInjection
  {
    void Inject(object instance);
    void Release(object instance);
  }
}