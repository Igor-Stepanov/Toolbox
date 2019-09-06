using System.Linq;
using Common.Extensions;
using FeaturesDI.Dependant;
using FeaturesDI.Registered;
using FeaturesDI.RegisterExpression;
using FeaturesDI.Static;

namespace FeaturesDI.Client
{
  public class Features
  {
    private readonly IFeatures _features;
    private readonly IDependants _dependants;

    public Features()
    {
      _features = new Registered.Features();
      _dependants = new Dependants();

      DI.Initialize(_features, _dependants);
    }

    public void Initialize() => _features
     .ForEach(x => x.Lifecycle.Start());

    public void Pause() => _features
     .Reverse()
     .ForEach(x => x.Lifecycle.Pause());

    public void Continue() => _features
       .ForEach(x => x.Lifecycle.Continue());

    public void Terminate()
    {
      DI.Terminate();
      
      _features
       .Reverse()
       .ForEach(x => x.Lifecycle.Stop());
      
      _features.Clear();
      _dependants.ReleaseAll();
    }
    
    public RegisterFeatureExpression<TFeature> Register<TFeature>(TFeature feature)
      where TFeature : Feature
    {
      _features.Add(feature);
      return new RegisterFeatureExpression<TFeature>(_features);
    }
  }
}