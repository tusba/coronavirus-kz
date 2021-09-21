using System;
using System.IO;
using System.Threading.Tasks;

namespace Tusba.Components.Logging
{
	public class FileLogger : InterfaceLogger
	{
		// project's root directory
		protected readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");
		// relative path from this.baseDir
		protected string dirPath = "";
		protected string fileName;

		private string fullDirName = "";
		private string fullFileName = "";

		public FileLogger(string fileName)
		{
			this.fileName = fileName;
			updateFilePath();
		}

		public string BaseDirectory => baseDir;

		public string Directory
		{
			get => fullDirName;

			set
			{
				dirPath = value;
				updateFilePath();
			}
		}

		/**
		 * @throws IOException
		 */
		public async Task Log(object target)
		{
			string logText = target.ToString() + Environment.NewLine;

			if (File.Exists(fullFileName))
			{
				await File.AppendAllTextAsync(fullFileName, logText);
			}
			else
			{
				await File.WriteAllTextAsync(fullFileName, logText);
			}
		}

		private void updateFilePath()
		{
			fullDirName = Path.Combine(baseDir, dirPath);
			fullFileName = Path.Combine(fullDirName, fileName);
		}
	}
}
