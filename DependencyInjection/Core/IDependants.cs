namespace DependencyInjection.Core
{
  internal interface IDependants
  {
    void Inject(object instance);
    void Release(object instance);
    
    void Clear();
  }
}