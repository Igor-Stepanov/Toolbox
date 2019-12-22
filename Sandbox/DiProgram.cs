using DIFeatures.Public;

namespace Sandbox
{
  public interface ITestFeature : IFeature
  {}
  
  public class TestFeature : Feature, ITestFeature
  {
    
  }
  
  public class DiProgram
  {
    public void Main()
    {
      var features = new Features();
      
      features
        .Let<ITestFeature>().Be<TestFeature>();
    }
  }
}