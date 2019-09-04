using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pocket.Benchmarks;

namespace Sandbox
{
  [Sample(times: 100)]
  public class ForeachSample
  {
    private static First3  First = new First3();
    private static Second3  Second = new Second3();

    [Run]
    public void Reflection()
    {
//      foreach (var method in Methods(Second.GetType()))
//        method.Invoke(Second, null);
    }

    [Run]
    public void Polymorphism()
    {
      Second.Method();
    }

    
  }
  public class First1
  {
    protected int I = 0;

    private void Method() =>
      I++;
  }
    
  public class First2 : First1
  {
    private void Method() =>
      I++;
  }
  
  public class First3 : First2
  {
    private void Method() =>
      I++;
  }

  public class Second1
  {
    protected int I = 0;

    public virtual void Method() =>
      I++;
  }
  
  public class Second2 : Second1
  {
    public override void Method()
    {
      base.Method();
      I++;
    }
  }
  
  public class Second3 : Second2
  {
    public override void Method()
    {
      base.Method();
      I++;
    }
  }
}
