// ReSharper disable All

using Sandbox.UDev.Correct;
using Sandbox.UDev.Correct.Liquid;
using Sandbox.UDev.Correct.Syrups;
using Sandbox.UDev.Correct.Syrups.Extensions;
using Sandbox.UDev.Wrong;

using static Sandbox.UDev.Correct.Bottle.BottleTypeId;

namespace Sandbox
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      var liquid = new Water()
       .With<HoneySyrup>()
       .With<MapleSyrup>();
    }
  }
}
