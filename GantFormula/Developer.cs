using GantFormula.Extensions;

namespace GantFormula
{
  public class Developer : Worker
  {
    protected override void Work()
    {
      var progress = Task.Develop();
      if (progress == Status.ReadyForQa)
      {
        Task.ReadyForQa();
        Task = null;
      }
    }

    protected override void Assign(JiraTask task) => 
      task.Status = Status.InProgress;
  }
}