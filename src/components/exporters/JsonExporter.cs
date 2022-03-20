using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tusba.Models;
using StreamUtil = Tusba.Components.Util.Stream;

namespace Tusba.Components.Exporters
{
	public class JsonExporter<T> : BaseExporter<T> where T : IConvertible
	{
		protected override async Task<string> ReturnContent()
		{
			using (var memoryStream = new MemoryStream())
			{
				await SerializeContent(memoryStream);

				return await StreamUtil.Read(memoryStream, ExportEncoding);
			}
		}

		protected override async Task<bool> StoreContentInternally()
		{
			using (var fileStream = File.Create(FileName))
			{
				try
				{
					await SerializeContent(fileStream);

					return true;
				}
				catch
				{
					return false;
				}
			}
		}

		private async Task SerializeContent(Stream stream)
		{
			await JsonSerializer.SerializeAsync<PostStats[]>(stream, Models);
		}
	}
}
