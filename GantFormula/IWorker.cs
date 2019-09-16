using System;
using System.Collections.Generic;

namespace GantFormula
{
  public interface IWorker
  {
    IEnumerable<WorkDay> WorkDays { get; }
    
    void Assign(JiraTask task);
    void Work(DateTime day);
  }
}