using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using DIFeatures.Public;
using DIFeatures.Public.Extensions;
using DIFeatures.RegisterExpression;
using GantFormula;
using static DIFeatures.RegisterExpression.Implementation;

namespace Sandbox
{
  public partial class Program
  {
    public interface ITestFeature1 : IFeature
    {
    }
    
    public interface ITestFeature2 : IFeature
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
      var gant = new GantSolutions(new List<Developer>{new Developer()}, null, tasks: Tasks.Fake());
      gant.Calculate();
      
      Debugger.Break();
    }

    [MemoryDiagnoser]
    public class FeaturesDIBenchmark
    {
      public int N;

      private Features _features;
      private TestFeature _testFeature;
      
      [GlobalSetup]
      public void Setup()
      {
        _features = new Features();
      }
      
      [IterationCleanup]
      public void IterationCleanup()
      {
        _features.Terminate();
        _features = new Features();
      }

      [Benchmark(OperationsPerInvoke = 1000000)]
      public void FeaturesDI()
      {
        var objects = new object[2000000];
        for (int i = 0; i < 2000000; i++)
        {
          objects[i] = new object();
        }
        
        Console.WriteLine(objects.GetType().Name);
        _features
          .Register(_testFeature)
          .AsImplementation(Of<ITestFeature1>());
      }
    }
  }
}
