using System;
using DI.Client;

namespace DI.Reference
{
  public interface IFeatureReference
  {
    Type Type { get; }
  }
  
  public class Feature<TReference> where TReference : class, IFeature
  {
    private TReference _reference;
    
    internal Feature()
    {
    }
    
    internal static Feature<TReference> New() =>
      new Feature<TReference>();

    public static implicit operator TReference(Feature<TReference> self) =>
      self._reference;
  }
}