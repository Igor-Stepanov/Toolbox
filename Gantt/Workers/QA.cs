using Gantt.Jobs;
using Gantt.Tasks;
using MessagePack;

namespace Gantt.Workers
{
  [MessagePackObject]
  public class QA : Worker
  {
    public QA(){}
    public QA(int id) : base(id){}

    protected override JobStatus WorkOn(JiraTask task) => 
      task.QAJob.Perform();
  }
}