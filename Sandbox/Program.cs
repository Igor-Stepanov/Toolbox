using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Extensions;
using Gantt;
using Gantt.Solutions;
using Sheets.Core;
using Sheets.Model;
using Sheets.Model.Rows;
using static System.Linq.Enumerable;

namespace Sandbox
{
  public class Program
  {
    private const string SpreadsheetId = "1-Wu_kdS2bXqr92ciJLhl6rzmPGeTv3QvE9LrjBMDvE4";
    private const string SetupSheet = "Setup";
    private const string ResultSheet = "Result";

    private static readonly ISheetsService Service = new GoogleSheetsService("Credentials/gant-client.json");

    public static void Main(string[] args)
    {
      var spreadsheet = new Spreadsheets(Service).Spreadsheet(SpreadsheetId);
      var setupSheet = spreadsheet.Sheet(SetupSheet);

      var setupRows = setupSheet.Cells.ToArray();
      
      var devCount =  int.Parse((string)setupRows[0].Raw[1]);
      var qaCount = int.Parse((string)setupRows[1].Raw[1]);
      
      var jiraTasks = setupRows.Skip(3)
        .Select(row => new JiraTask((string)row.Raw[0], int.Parse((string)row.Raw[1]), int.Parse((string)row.Raw[2])))
        .ToList();
      
      var devs = new Developer[devCount];
      var qas = new List<Qa>(qaCount);

      Range(0, devCount).ForEach(i => devs[i] = new Developer(id: i));
      Range(0, qaCount).ForEach(x => qas.Add(new Qa(id: x)));
      
      var gant = new GantSolutions(devs, qas, jiraTasks);
      
      gant.Calculate();
      
      while (gant.RunningTasks > 0)
      {
        Console.WriteLine($"Running: {gant.RunningTasks} Total: {gant.TotalTasks}");
        Thread.Sleep(5000);
      }

      var solution = gant.Best;

      var days = new object[] {"Dates:"}.Concat(Range(1, solution.Days).Cast<object>()).ToArray();

      var nextRowId = 0;
      var rows = new List<IRow> {Row.AsRow(days, nextRowId++)};

      foreach (var worker in solution.Workers)
      {
        var values = new List<object> {$"{(worker is Developer ? "Developer" : "Qa")} #{worker.Id}:"};

        for (var i = 1; i <= solution.Days; i++)
        {
          var workDay = worker.WorkDays.FirstOrDefault(x => x.Day == (int) days[i]);
          values.Add(workDay?.Task);
        }
        
        rows.Add(Row.AsRow(values, nextRowId++));
      }

      rows.Add(Row.AsRow( new object[]{$"Total iterations: {gant.TotalTasks}"}, nextRowId++));
      spreadsheet.Update(SpreadsheetId, ResultSheet, rows);
    }
  }
}
