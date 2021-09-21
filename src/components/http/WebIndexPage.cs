using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tusba.Components.Http
{
	public class WebIndexPage : InterfaceHttpGet
	{
		private readonly HttpClient Client = new HttpClient();

		private readonly string baseUrl;

		public WebIndexPage(string baseUrl)
		{
			this.baseUrl = baseUrl;

			if (this.baseUrl.StartsWith("https"))
			{
				// fix: The request was aborted: Could not create SSL/TLS secure channel.
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			}
		}

		public void Dispose()
		{
			Client.Dispose();
		}

		/**
		 * @throws Exception
		 */
		public async Task<string> Get(string? postId)
		{
			string url = baseUrl + (postId is null ? "" : $"/{postId}");

			return await Client.GetStringAsync(url);
		}
	}
}
