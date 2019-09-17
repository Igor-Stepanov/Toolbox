using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common.Extensions;
using GantFormula;
using static System.Linq.Enumerable;

namespace Sandbox
{
  public class Program
  {
    private const int QaCount = 1;
    private const int DevCount = 1;
    
    public static void Main(string[] args)
    {
      var developers = new List<Developer>(DevCount);
      var qas = new List<Qa>(QaCount);

      Range(0, DevCount)
        .ForEach(x => developers.Add(new Developer(x)));
      
      Range(0, QaCount)
        .ForEach(x => qas.Add(new Qa(x)));
      
      var gant = new GantSolutions(developers, qas, Tasks.Fake());
      
      gant.Calculate();

      int? last = null;
      foreach (var duration in gant.All.Select(x => (x.Day - DateTime.Today).TotalDays).OrderByDescending(x => x))
      {
        if ((int) duration != last)
        {
          last = (int?) duration;
          Console.WriteLine(last.Value);
        }
      }
      Debugger.Break();
    }
  }
}
