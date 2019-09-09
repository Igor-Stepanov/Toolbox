using System;
using DIFeatures.Dependant;
using DIFeatures.Errors;
using DIFeatures.Registered;
using DIFeatures.ThreadSafe;

namespace DIFeatures.Static
{
  internal class DI
  {
    public static DI Features { get; private set; }

    private readonly IErrors _errors;
    private readonly IFeatures _features;
    private readonly IDependants _dependants;

    private readonly ThreadSafeSection _threadSafe;

    private DI(IErrors errors, IFeatures features, IDependants dependants)
    {
      _errors = errors;
      _features = features;
      _dependants = dependants;

      _threadSafe = new ThreadSafeSection();
    }

    public void InjectInto(object instance)
    {
      try
      {
        using (_threadSafe.Section)
          _dependants.Inject(instance, _features);
      }
      catch (Exception exception)
      {
        _errors.Handle(exception);
      }
    }

    public void Release(object instance)
    {
      try
      {
        using (_threadSafe.Section)
          _dependants.Release(instance);
      }
      catch (Exception exception)
      {
        _errors.Handle(exception);
      }
    }

    internal static void Initialize(IErrors errors, IFeatures features, IDependants dependants)
    {
      if (Features != null)
        throw new InvalidOperationException("Multiple instances of DI is not allowed.");

      Features = new DI(errors, features, dependants);
    }

    internal static void Terminate()
    {
      if (Features == null)
        throw new InvalidOperationException("Instance of DI is terminated already.");

      Features = null;
    }
  }
}