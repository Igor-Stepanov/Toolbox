using DI;
using DI.RegisterExpression;

namespace Sandbox
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      var features = new Features();

      features.Register(new TestFeature())
       .As(Implementation.Of<ITestFeature>())
       .As(Implementation.Of<ITestFeature2>());

    }
    
    public interface ITestFeature : IFeature
    {
      
    }
    public interface ITestFeature2 : IFeature
    {
      
    }

    
    public interface IOtherFeature : IFeature
    {
      
    }

    
    public class TestFeature : Feature, ITestFeature2, ITestFeature
    {
      
    }
  }
}
