using System;
using System.IO;

namespace Tusba.Components.Logging
{
	public class FileLogger : InterfaceLogger
	{
		// project's root directory
		protected readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");
		// relative path from this.baseDir
		protected string dirPath = "";
		protected string fileName;

		private string fullFileName;

		public FileLogger(string fileName)
		{
			this.fileName = fileName;
			updateFilePath();
		}

		public string BaseDirectory => baseDir;

		public string Directory
		{
			set
			{
				dirPath = value;
				updateFilePath();
			}
		}

		/**
		 * @throws IOException
		 */
		public void Log(object target)
		{
			string logText = target.ToString() + Environment.NewLine;

			if (File.Exists(fullFileName))
			{
				File.AppendAllText(fullFileName, logText);
			}
			else
			{
				File.WriteAllText(fullFileName, logText);
			}
		}

		private void updateFilePath()
		{
			fullFileName = Path.Combine(baseDir, dirPath, fileName);
		}
	}
}
