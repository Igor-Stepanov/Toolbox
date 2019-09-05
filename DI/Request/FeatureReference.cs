using DI.Client;

namespace DI.Request
{
  public delegate ref TReference Feature<TReference>()
    where TReference : class, IFeature;
}