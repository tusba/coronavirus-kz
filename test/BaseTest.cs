using System;
using System.IO;

namespace test
{
  public abstract class BaseTest
  {
		// project's root directory
		protected readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");

		// tests' root directory
		protected readonly string testDir;

		public BaseTest()
		{
			testDir = Path.Combine(baseDir, @"test");
		}
  }
}
