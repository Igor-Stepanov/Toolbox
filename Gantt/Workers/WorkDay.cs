using MessagePack;

namespace Gantt.Workers
{
  [MessagePackObject]
  public class WorkDay
  {
    [Key(0)] public int Day;
    [Key(1)] public string Task;
  }
}