using System;
using System.Linq;
using System.Threading;
using Common.Extensions;
using Gantt.Solutions;
using Gantt.Tasks;
using Gantt.Workers;
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

    private static readonly ISheetsService Service = new GoogleSheetsService("Credentials/gant-client.json");

    public static void Main(string[] args)
    {
      var spreadsheet = Service.Spreadsheets.OneWith(SpreadsheetId);
      var sheet = spreadsheet.Sheet(SetupSheet);
      
      var devCount =  int.Parse(sheet[1, 0]);
      var qaCount = int.Parse(sheet[1, 1]);
      
      var devs = new Developer[devCount];
      var qas = new QA[qaCount];

      Range(0, devCount).ForEach(i => devs[i] = new Developer(id: i));
      Range(0, qaCount).ForEach(i => qas[i] = new QA(id: i));
      
      var tasks = sheet.Rows
        .Skip(3)
        .Select(x => new JiraTask(x[0], int.Parse(x[1]), int.Parse(x[2])))
        .ToArray();
      
      var solutions = new GantSolutions(devs, qas, tasks);
      solutions.Calculate();
      
      while (solutions.RunningTasks > 0)
      {
        Console.WriteLine($"Running: {solutions.RunningTasks} Total: {solutions.TotalTasks}");
        Thread.Sleep(5000);
      }

      var result = spreadsheet.Sheet(ResultSheet);

      var solution = solutions.Best;
      
      result[0, 0] = "Dates:";
      Range(1, solution.Days).ForEach(i => result[i, 0] = i);

      foreach (var (worker, workerIndex) in solution.Workers.Select((x, i) => (Worder: x, Index: i + 1)))
      foreach (var (task, day) in worker.WorkDays.Select((x, i) => (Task: x.Task, Index: i)))
        result[day, workerIndex] = task;
      
      result.Save();
    }
  }
}
