using System;
using System.IO;
using IoDirectory = System.IO.Directory;

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

		public virtual string Directory
		{
			get => fullDirName;

			set
			{
				dirPath = value;
				UpdateFilePath();
			}
		}

		public virtual string FileName
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
			fullDirName = Path.Combine(baseDir, @dirPath);
			fullFileName = Path.Combine(fullDirName, @fileName);
		}

		private class FileStorageProvider : FileStorage
		{
			private bool Absolute { get; }

			public FileStorageProvider(bool absolutePaths = false) : base()
			{
				Absolute = absolutePaths;
			}

			public override string Directory
			{
				get => base.Directory;

				set
				{
					if (Absolute)
					{
						fullDirName = value;
					}
					else
					{
						base.Directory = value;
					}
				}
			}

			public override string FileName
			{
				get => base.FileName;

				set
				{
					if (Absolute)
					{
						fullFileName = value;
					}
					else
					{
						base.FileName = value;
					}
				}
			}
		}

		public static bool ProvideDirectory(string dirName, bool absolutePath = false)
		{
			FileStorage fs = new FileStorageProvider(absolutePath);
			fs.Directory = @dirName;
			string dirPath = fs.Directory;

			return IoDirectory.Exists(dirPath) || IoDirectory.CreateDirectory(dirPath).Exists;
		}
	}
}
