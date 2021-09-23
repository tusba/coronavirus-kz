using System;
using System.IO;

namespace Tusba.Components.FileSystem
{
	public abstract class FileStorage
	{
		// project's root directory
		protected readonly string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../..");
		// relative path from this.baseDir
		private string dirPath = "";
		private string fileName = "";

    // absolute paths to directory and file respectively
		private string fullDirName = "";
		private string fullFileName = "";

		public string BaseDirectory => baseDir;

		public string Directory
		{
			get => fullDirName;

			set
			{
				dirPath = value;
				UpdateFilePath();
			}
		}

		public string FileName
		{
			get => fullFileName;

			set
			{
				fileName = value;
				UpdateFilePath();
			}
		}

		protected void UpdateFilePath()
		{
			fullDirName = Path.Combine(baseDir, dirPath);
			fullFileName = Path.Combine(fullDirName, fileName);
		}
	}
}
