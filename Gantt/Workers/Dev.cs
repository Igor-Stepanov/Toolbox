using Gantt.Jobs;
using Gantt.Tasks;
using MessagePack;

namespace Gantt.Workers
{
  [MessagePackObject]
  public class Dev : Worker
  {
    public Dev(){}
    public Dev(int id) : base(id){}
    
    protected override JobStatus WorkOn(JiraTask task) => 
      task.DevJob.Perform();
  }
}