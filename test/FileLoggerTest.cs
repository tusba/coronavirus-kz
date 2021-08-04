using System;
using System.IO;
using Xunit;
using Tusba.Components.Logging;

namespace test
{
	public class FileLoggerTest
	{
		// project's root directory
		private readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");

		[Fact]
		public void NonExistingDirectoryTest()
		{
			string logDir = @"log-test", logPath;

			AssertDirectoryNotExist(logDir, out logPath);

			Assert.Throws<DirectoryNotFoundException>(() => {
				InterfaceLogger logger = new FileLogger(@"test-1.log");
				((FileLogger) logger).Directory = logDir;
				logger.Log("Some test data");
			});

			Assert.False(Directory.Exists(logPath));
		}

		[Fact]
		public void LogTest()
		{
			string logDir = @"log-test", logPath, fileName = @"test-1.log";

			AssertDirectoryNotExist(logDir, out logPath);
			string filePath = Path.Combine(logPath, fileName);

			string eol = Environment.NewLine, fileContent;
			string[] content = {
				"Some test data",
				"More test data",
				"Qwerty 123456"
			};

			DirectoryInfo dirInfo = Directory.CreateDirectory(logPath);
			InterfaceLogger logger = new FileLogger(fileName);
			((FileLogger) logger).Directory = logDir;
			logger.Log(content[0]);

			Assert.True(File.Exists(filePath));

			fileContent = File.ReadAllText(filePath);
			Assert.Equal(content[0] + eol, fileContent);

			logger.Log(content[1]);
			logger.Log(content[2]);
			fileContent = File.ReadAllText(filePath);
			Assert.Equal(String.Join(eol, content) + eol, fileContent);

			CleanUp(dirInfo);
		}

		private void AssertDirectoryNotExist(string dirName, out string dirPath)
		{
			dirPath = Path.Combine(baseDir, dirName);
			Assert.False(Directory.Exists(dirPath));
		}

		private void CleanUp(DirectoryInfo dirInfo)
		{
			if (dirInfo.Exists)
			{
				dirInfo.Delete(true);
			}

			Assert.False(dirInfo.Exists);
		}
	}
}
