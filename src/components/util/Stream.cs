using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Tusba.Components.Util
{
	public class Stream
	{
		public static Encoding DefaultEncoding = new UTF8Encoding(false);

		public static async Task<string> Read(MemoryStream memoryStream, Encoding? encoding = null)
		{
				// rewind to the beginning after possible writing to the stream
				memoryStream.Seek(0, SeekOrigin.Begin);

				int contentSize = (int) memoryStream.Length;
				var byteContent = new byte[contentSize];
				await memoryStream.ReadAsync(byteContent, 0, contentSize);

				return (encoding ?? DefaultEncoding).GetString(byteContent);
		}
	}
}
