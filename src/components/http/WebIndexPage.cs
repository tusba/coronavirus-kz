using System.Net;
using System.Net.Http;

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
		public string Get(string? postId)
		{
			string url = baseUrl + (postId is null ? "" : $"/{postId}");

			return Client.GetStringAsync(url).Result;
		}
	}
}
