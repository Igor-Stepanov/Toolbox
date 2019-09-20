using Gantt.Jobs;
using MessagePack;

namespace Gantt
{
  [MessagePackObject]
  public class Developer : Worker
  {
    public Developer(){}
    public Developer(int id) : base(id){}
    
    protected override JobStatus WorkOn(JiraTask task) => 
      task.DevJob.Perform();
  }
}