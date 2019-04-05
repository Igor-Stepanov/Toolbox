using Pocket.Benchmarks;

namespace Sandbox
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      Benchmark.OfAssembly().Execute();
    }
  }
}
