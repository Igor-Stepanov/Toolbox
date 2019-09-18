using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Extensions;
using GantFormula;
using Sheets.Core;
using Sheets.Model;
using static System.Linq.Enumerable;

namespace Sandbox
{
  public class Program
  {
    private const string SpreadsheetId = "1-Wu_kdS2bXqr92ciJLhl6rzmPGeTv3QvE9LrjBMDvE4";
    private const string SetupSheet = "Setup";
    private const string ResultSheet = "Result";

    public static void Main(string[] args)
    {
      var sheets = new GoogleSheetsService("Credentials/gant-client.json");

      var setupRows = sheets.FetchRows(SpreadsheetId, SetupSheet).ToArray();
      var devCount =  int.Parse((string)setupRows[0].Raw[1]);
      var qaCount = int.Parse((string)setupRows[1].Raw[1]);
      
      var jiraTasks = setupRows
        .Skip(3)
        .Select(row => new JiraTask((string)row.Raw[0], int.Parse((string)row.Raw[1]), int.Parse((string)row.Raw[2])))
        .ToList();

      var developers = new List<Developer>(devCount);
      var qas = new List<Qa>(qaCount);

      Range(0, devCount)
        .ForEach(x => developers.Add(new Developer(x)));
      
      Range(0, qaCount)
        .ForEach(x => qas.Add(new Qa(x)));
      
      var gant = new Gant(developers, qas, jiraTasks);
      
      gant.Calculate();
      
      while (gant.RunningTasks > 0)
      {
        Console.WriteLine($"Running: {gant.RunningTasks} Total: {gant.TotalTasks}");
        Thread.Sleep(5000);
      }

      var solution = gant.Best;

      var days = new object[] {"Dates:"}.Concat(Range(1, solution.Day).Cast<object>()).ToArray();

      var nextRowId = 0;
      var rows = new List<IRow> {Row.Create(days, nextRowId++)};

      foreach (var worker in solution.Workers)
      {
        var values = new List<object> {$"{(worker is Developer ? "Developer" : "Qa")} #{worker.Id}:"};

        for (var i = 1; i <= solution.Day; i++)
        {
          var workDay = worker.WorkDays.FirstOrDefault(x => x.Day == (int) days[i]);
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

      rows.Add(Row.Create( new object[]{$"Total iterations: {gant.TotalTasks}"}, nextRowId++));

      new GoogleSheetsService("Credentials/gant-client.json")
       .Update(SpreadsheetId, ResultSheet, rows);
      
      Console.WriteLine();
    }
  }
}
