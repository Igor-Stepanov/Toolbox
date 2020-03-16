using System.Diagnostics;
using DI.Public;

namespace DI
{
  public static class Program
  {
    public static void Main()
    {
      var container = new Container();
      
      container
        .Let<ITestFeature>()
        .Be<TestFeature>();
      
      
      var t = new Test();
      t.Kek();


      container.Clear();
    }
  }

  public class Test
  {
    private ITestFeature TestFeature => Resolve.One<ITestFeature>();

    public void Kek()
    {
      var a = TestFeature;
      Debugger.Break();
    }
  }

  public interface ITestFeature
  {
  }
  
  public class TestFeature : ITestFeature
  {
  }
}