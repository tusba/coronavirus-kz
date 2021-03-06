using System;
using Xunit;
using Tusba.Components.Http;

namespace test
{
	public class WebIndexPageTest : BaseTest
	{
		[Theory]
		[InlineData("about:blank")]
		[InlineData("ftp://127.0.0.1")]
		public void InvalidUrlTest(string url)
		{
			using (InterfaceHttpGet client = new WebIndexPage(url))
			{
				Assert.ThrowsAsync<ArgumentException>(async () => {
					await client.Get();
				});
			}
		}

		[Theory]
		[InlineData("http://no-such-url.qwe")]
		[InlineData("https://no-such-url.qwe")]
		public void NonExistentUrlTest(string url)
		{
			using (InterfaceHttpGet client = new WebIndexPage(url))
			{
				Assert.ThrowsAsync<AggregateException>(async () => {
					await client.Get();
				});
			}
		}

		[Theory]
		[InlineData("http://httpforever.com/")]
		[InlineData("https://github.com")]
		public async void GetTest(string url)
		{
			using (InterfaceHttpGet client = new WebIndexPage(url))
			{
				string responseBody = await client.Get();
				Assert.True(responseBody.Length > 0);
			}
		}
	}
}
