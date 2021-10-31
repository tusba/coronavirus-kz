using System;
using System.IO;

namespace test
{
  public abstract class BaseTest
  {
		// project's root directory
		protected readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");
  }
}
