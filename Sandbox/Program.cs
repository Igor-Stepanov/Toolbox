using System;
using Common.Extensions;
using Pocket.Benchmarks;

namespace Sandbox
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      Benchmark.OfAssembly().Execute();
      int? value = null;
      
      
      Console.WriteLine(value.Or(10));
    }
  }
}
