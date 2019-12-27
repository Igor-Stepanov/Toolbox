namespace DependencyInjection.Model.Lifecycles
{
  public enum State
  {
    Created = 0,
    Initializing = 1,
    Initialized = 2,
    Pausing = 3,
    Paused = 4,
    Continuing = 5,
    Continued = 6,
    Terminating = 7,
    Terminated = 8,
  }
}