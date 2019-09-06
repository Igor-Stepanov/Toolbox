using System;

namespace FeaturesDI.Dependencies
{
  internal interface IDependantTypes
  {
    DependantType OneOf(Type type);
  }
}