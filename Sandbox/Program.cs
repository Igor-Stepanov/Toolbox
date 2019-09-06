using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FeaturesDI.Client;
using FeaturesDI.Client.Extensions;
using FeaturesDI.RegisterExpression;

namespace Sandbox
{
  internal class Program
  {
    public interface ITestFeature1 : IFeature
    {
      
    }
    
    public interface ITestFeature2
    {
      
    }

    
    public class TestFeature : Feature, ITestFeature1, ITestFeature2
    {
    }
    
    public class TestDependant
    {
      [Inject] private readonly ITestFeature1 _test;

      public TestDependant()
      {
        this.InjectDependencies();
      }

      public void Terminate() =>
        this.ReleaseDependencies();
    }


    public static void Main(string[] args)
    {
      var features = new Features();

      features.Register(new TestFeature())
       .As(Implementation.Of<ITestFeature1>());
      

      Debugger.Break();
    }
  }
}
