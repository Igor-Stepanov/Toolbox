using System.Collections.Generic;
using GantFormula;

namespace Sandbox
{
  public static class Tasks
  {
    public static List<JiraTask> Fake() =>
      new List<JiraTask>
      {
        new JiraTask
        {
          Name = "HarderToDevelop",
          DevelopmentDays = 3,
          QaDays = 1
        },
        new JiraTask
        {
          Name = "HarderToDevelop2",
          DevelopmentDays = 3,
          QaDays = 1
        },
        new JiraTask
        {
          Name = "HarderToQa",
          DevelopmentDays = 1,
          QaDays = 3
        },
        new JiraTask
        {
          Name = "HarderToQa2",
          DevelopmentDays = 1,
          QaDays = 3
        },
        new JiraTask
        {
          Name = "FractionWars",
          DevelopmentDays = 5,
          QaDays = 5
        }
      };
  }
}