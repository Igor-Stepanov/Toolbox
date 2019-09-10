namespace DIFeatures.DependencyInjection
{
  public interface IDependencyInjection
  {
    void InjectInto(object instance);
    void Release(object instance);
  }
}