using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
  public class JsonAttribute : Attribute
  {}
  
  public class KeyAttribute : Attribute
  {}


  public class Simple
  {
    [Json]
    public int _field;
  }

  public static class DirectoryInfoExtensions
  {
    public static IEnumerable<DirectoryInfo> Kek(this DirectoryInfo self)
    {
      if (self == null)
        yield break;
      
      do
      {
        yield return self;
        self = self.Parent;
      } 
      while (self != null);
    }
  }
  
  class Program
  {
    static void Main(string[] args)
    {
      Debugger.Break();
    }
  }
}