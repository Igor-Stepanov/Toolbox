namespace DependencyInjection.Model.Lifecycles.Extensions
{
  public static class LifecycleStateExtensions
  {
    public static void To(this ref State self, State state) =>
      self = state;
  }
}