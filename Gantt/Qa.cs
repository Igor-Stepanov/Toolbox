using Gantt.Jobs;
using MessagePack;

namespace Gantt
{
  [MessagePackObject]
  public class Qa : Worker
  {
    public Qa(){}
    public Qa(int id) : base(id){}

    protected override JobStatus WorkOn(JiraTask task) => 
      task.QaJob.Perform();
  }
}