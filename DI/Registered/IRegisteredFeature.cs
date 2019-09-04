using System;
using Common.Collections.OrderedDictionary;

namespace DI.Registered
{
  internal interface IRegisteredFeature
  {
    Type Type { get; }
  }
}