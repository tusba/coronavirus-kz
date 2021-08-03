using System;
using System.IO;
using Xunit;
using Tusba.Components.Logging;

namespace test
{
	public class FileLoggerTest
	{
		// project's root directory
		protected readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");

		[Fact]
		public void NonExistingDirectoryTest()
		{
			string logDir = @"log-test";
			string logPath = Path.Combine(baseDir, logDir);

			Assert.False(Directory.Exists(logPath));

			Assert.Throws<DirectoryNotFoundException>(() => {
				InterfaceLogger logger = new FileLogger("test-1.log");
				((FileLogger) logger).Directory = logDir;
				logger.Log("Some test data");
			});

			Assert.False(Directory.Exists(logPath));
		}

		[Fact]
		public void LogTest()
		{
		}
	}
}
