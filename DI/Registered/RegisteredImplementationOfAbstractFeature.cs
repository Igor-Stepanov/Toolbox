using System;
using DI.Client;

namespace DI.Registered
{
  internal class RegisteredImplementationOf<TAbstractFeature> : IRegisteredFeature
    where TAbstractFeature : IFeature
  {
    Type IRegisteredFeature.Type => typeof(TAbstractFeature);
    IFeature IRegisteredFeature.Feature => Feature;

    protected readonly TAbstractFeature Feature;

    public RegisteredImplementationOf(TAbstractFeature feature) => 
      Feature = feature;

    public override int GetHashCode() => 
      typeof(TAbstractFeature).GetHashCode();

    public override bool Equals(object other) => 
      typeof(TAbstractFeature).Equals(other);
    
    public static explicit operator TAbstractFeature(RegisteredImplementationOf<TAbstractFeature> self) =>
      self.Feature;
  }
}