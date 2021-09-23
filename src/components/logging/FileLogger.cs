using System;
using System.IO;
using System.Threading.Tasks;
using Tusba.Components.FileSystem;

namespace Tusba.Components.Logging
{
	public class FileLogger : FileStorage, InterfaceLogger
	{
		public FileLogger(string fileName)
		{
			FileName = fileName;
		}

		/**
		 * @throws IOException
		 */
		public async Task Log(object target)
		{
			string logText = target.ToString() + Environment.NewLine;

			if (File.Exists(FileName))
			{
				await File.AppendAllTextAsync(FileName, logText);
			}
			else
			{
				await File.WriteAllTextAsync(FileName, logText);
			}
		}
	}
}
