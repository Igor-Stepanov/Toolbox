using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sandbox
{
  internal class Program
  {
    public class Test
    {
      public override bool Equals(object obj) =>
        true;

      public override int GetHashCode() =>
        0;
    }


    public static void Main(string[] args)
    {
      var t = new Test();

      var taskI = Task.Run(() => RuntimeHelpers.GetHashCode(t));

      var i = taskI.Result;
      var i2 = RuntimeHelpers.GetHashCode(t);

      Debugger.Break();
    }
  }
}
