using System;
using Xunit;
using Tusba.Components.Http;

namespace test
{
	public class WebIndexPageTest
	{
		[Theory]
		[InlineData("about:blank")]
		[InlineData("ftp://127.0.0.1")]
		public void InvalidUrlTest(string url)
		{
			InterfaceHttpGet client = new WebIndexPage(url);

			Assert.Throws<ArgumentException>(() => {
				client.Get(null);
			});
		}

		[Theory]
		[InlineData("http://no-such-url.qwe")]
		[InlineData("https://no-such-url.qwe")]
		public void NonExistentUrlTest(string url)
		{
			InterfaceHttpGet client = new WebIndexPage(url);

			Assert.Throws<AggregateException>(() => {
				client.Get(null);
			});
		}

		[Theory]
		[InlineData("http://nikvel.ru")]
		[InlineData("https://github.com")]
		public void GetTest(string url)
		{
			InterfaceHttpGet client = new WebIndexPage(url);

			string responseBody = client.Get(null);

			Assert.True(responseBody.Length > 0);
		}
	}
}
