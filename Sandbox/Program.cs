using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common.Extensions;
using GantFormula;
using Sheets.Core;
using Sheets.Model;
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

      var solution = gant.All.OrderBy(x => x.Day).First();
      var lastDay = solution.Day;      
      
      var rows = new List<IRow>();
      
      var days = new List<DateTime>();
      days.Add(DateTime.Today);

      while (days.Last() < lastDay)
          days.Add(days.Last().AddDays(1));

      var nextRowId = 0;
      
      rows.Add(Row.Create(days.Select(x => x.Date.ToString()).Cast<object>(), nextRowId++));

      foreach (var developer in solution.Developers)
      {
        var values = new List<object>();
        values.Add(developer.Id);
        
        for (var i = 0; i < days.Count; i++)
        {
          var workDay = developer.WorkDays.FirstOrDefault(x => x.Day == days[i]);
          if (workDay != null)
          {
            values.Add(workDay.Task);
          }
          else
          {
            values.Add(string.Empty);
          }
        }
        
        rows.Add(Row.Create(values, nextRowId++));
      }      

      new GoogleSheetsService("Credentials/gant-client.json")
       .UpdateRows("1-Wu_kdS2bXqr92ciJLhl6rzmPGeTv3QvE9LrjBMDvE4", "Result", rows);
      

      Debugger.Break();
    }
  }
}
