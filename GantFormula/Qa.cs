using static GantFormula.Status;

namespace GantFormula
{
  public class Qa : Worker
  {
    public class State
    {
      
    }
    
    protected override void Work()
    {
      var progress = Task.AssureQuality();
      if (progress == Done)
      {
        Task.Done();
        Task = null;
      }
    }

    protected override void Assign(JiraTask task) => 
      task.Status = InQa;
  }
}