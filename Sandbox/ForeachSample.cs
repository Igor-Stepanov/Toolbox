using System.Linq;
using Pocket.Benchmarks;

namespace Sandbox
{
  [Sample(times: 10000)]
  public class ForeachSample
  {
    private static int[] Array = new int[10_000];


    [Run]
    public void SimpleArray()
    {
      var sum = 0;
      for (var i = 0; i < Array.Length; i++)
      {
        sum += Array[i];
      }
    }

    [Run]
    public void JaggedArray()
    {
      var sum = Array.Sum();
    }
  }
}
