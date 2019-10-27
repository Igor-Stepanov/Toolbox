using System.Diagnostics;
using System.Linq;
using Common.Extensions;
using Gantt.Solutions;
using Gantt.Tasks;
using Gantt.Workers;
using Sheets.Core;
using Sheets.Model;
using static System.Console;
using static System.Linq.Enumerable;
using static System.Threading.Thread;

namespace Sandbox
{
  public class Program
  {
    private const string Gant = "1-Wu_kdS2bXqr92ciJLhl6rzmPGeTv3QvE9LrjBMDvE4";
    private const string SetupSheet = "Setup";
    private const string Result = "Result";

    private static readonly ISpreadsheets Spreadsheets = new GoogleSpreadsheets("Credentials/gant-client.json");

    public static void Main(string[] args)
    {
      //InputFromSpreadsheet(out var devs, out var qas, out var tasks);
      Input(out var devs, out var qas, out var tasks);

      var solutions = new GantSolutions(devs, qas, tasks);
      solutions.Calculate();
      
      while (solutions.Running > 0)
      {
        WriteLine($"Running: {solutions.Running} Total: {solutions.Total}");
        Sleep(5000);
      }

      //SaveResultsInSpreadsheet(solutions);
      Debugger.Break();
    }

    private static void InputFromSpreadsheet(out Dev[] devs, out QA[] qas, out JiraTask[] tasks)
    {
      var sheet = Spreadsheets
        .Spreadsheet(Gant)
        .Sheet(SetupSheet);

      var devCount = int.Parse(sheet[1, 0]);
      var qaCount = int.Parse(sheet[1, 1]);

      devs = new Dev[devCount];
      qas = new QA[qaCount];

      for (var id = 0; id < devCount; id++)
        devs[id] = new Dev(id);

      for (var id = 0; id < qaCount; id++)
        qas[id] = new QA(id);

      tasks = sheet.Rows
        .Skip(3)
        .Select(x => new JiraTask(x[0], int.Parse(x[1]), int.Parse(x[2])))
        .ToArray();
    }

    private static void SaveResultsInSpreadsheet(IGanttSolutions solutions)
    {
      var result = Spreadsheets
        .Spreadsheet(Gant)
        .Sheet(Result);

      var solution = solutions.Best;

      result[0, 0] = "Dates:";
      Range(1, solution.Days).ForEach(i => result[i, 0] = i);

      foreach (var (worker, workerIndex) in solution.Workers.Select((x, i) => (Worder: x, Index: i + 1)))
      foreach (var (task, day) in worker.WorkDays.Select((x, i) => (Task: x.Task, Index: i)))
        result[day, workerIndex] = task;

      result.Save();
    }
    
    private static void Input(out Dev[] devs, out QA[] qas, out JiraTask[] tasks)
    {
      devs = new Dev[]
      {
        new Dev
        {
        }, 
      };
    }
  }
}
