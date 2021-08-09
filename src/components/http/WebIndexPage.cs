using System.Net;
using System.Net.Http;

namespace Tusba.Components.Http
{
	public class WebIndexPage : InterfaceHttpGet
	{
		private readonly HttpClient Client = new HttpClient();

		private readonly string baseUrl;

		public WebIndexPage(string baseUrl) => this.baseUrl = baseUrl;

		public string Get(string? postId)
		{
			string url = baseUrl + (postId is null ? "" : "/" + postId);
			return url;
		}
	}
}
