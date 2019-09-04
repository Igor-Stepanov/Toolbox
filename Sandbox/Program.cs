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

      features.Register(new TestFeature());
      
      Benchmark.OfAssembly().Execute();
    }
    
    public interface ITestFeature : IFeature
    {
      
    }
    
    public class TestFeature : ITestFeature
    {
      
    }
  }
}
