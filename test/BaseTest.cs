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

		protected void CleanUp(string directoryName, bool onlyIfExists = false)
		{
			string directoryPath = Path.Combine(baseDir, directoryName);

			if (!onlyIfExists || Directory.Exists(directoryPath))
			{
				Directory.Delete(directoryPath, true);
			}
		}
  }
}
