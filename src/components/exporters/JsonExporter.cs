using System;
using System.Threading.Tasks;

namespace Tusba.Components.Exporters
{
	public class JsonExporter<T> : BaseExporter<T> where T : IConvertible
	{
		protected override async Task<string> ReturnContent()
		{
			return await Task.Run(() => "TODO JSON");
		}

		protected override async Task<bool> StoreContent()
		{
			return await Task.Run(() => false);
		}
	}
}
