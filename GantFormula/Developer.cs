using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public class Developer : Worker
  {
    public Developer() : base() { }
    public Developer(int id) : base(id) { }
    
    protected override bool Work(JiraTask task) => 
      task.Develop();
  }
}