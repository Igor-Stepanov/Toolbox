using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using DIFeatures.Public;
using DIFeatures.Public.Extensions;
using DIFeatures.RegisterExpression;
using static DIFeatures.RegisterExpression.Implementation;

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
    
    public abstract class Dependant
    {
      protected Dependant() => 
        this.InjectDependencies();
    }
    
    public class TestDependant : Dependant
    {
      [Inject] private readonly ITestFeature1 _test;
    }

    public static void Main(string[] args)
    {
      var features = new Features();

      features
       .Register(new TestFeature())
       .AsImplementation(Of<ITestFeature1>());
      
      var testDependant = new TestDependant();
      features.Terminate();
      

      Debugger.Break();
    }
  }
}
