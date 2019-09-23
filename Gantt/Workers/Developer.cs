using Gantt.Jobs;
using Gantt.Tasks;
using MessagePack;

namespace Gantt.Workers
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