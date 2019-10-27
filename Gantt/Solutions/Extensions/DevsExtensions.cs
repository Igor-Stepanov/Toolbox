using Gantt.Workers;

namespace Gantt.Solutions.Extensions
{
  public static class DevsExtensions
  {
    public static Dev[] CloneAll(this Dev[] self)
    {
      var copy = new Dev[self.Length];
      for (var i = 0; i < self.Length; i++)
        copy[i] = self[i].Clone();
    }
  }
}