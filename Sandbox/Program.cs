using DI;
using DI.RegisterExpression;
using Pocket.Benchmarks;

namespace Sandbox
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      var features = new Features();

      features.Register(new TestFeature())
        .As(Implementation.Of<ITestFeature>());
      
      Benchmark.OfAssembly().Execute();
    }
    
    public interface ITestFeature 
    {
      
    }
    
    public class TestFeature : Feature, ITestFeature
    {
      
    }
  }
}
