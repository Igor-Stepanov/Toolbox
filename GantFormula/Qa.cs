using MessagePack;

namespace GantFormula
{
  [MessagePackObject]
  public class Qa : Worker
  {
    public Qa() : base() { }
    public Qa(int id) : base(id) { }

    protected override bool Work(JiraTask task) => 
      task.Test();
  }
}