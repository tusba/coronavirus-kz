using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using StringUtil = Tusba.Components.Util.String;
using Tusba.Models;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Exporters
{
	public class XmlExporter<T> : InterfaceExporter<T> where T : IConvertible
	{
		private const string ELEMENT_ROOT = "PostStats";
		private const string ELEMENT_NODE = "Entry";
		private const string ELEMENT_REGION = "Region";
		private const string ELEMENT_QUANTITY = "Quantity";

		protected PostStats[] Models { get; set; } = new PostStats[] { };
		protected XElement XmlTree { get; set; } = new XElement(ELEMENT_ROOT);

		public async Task<T> ExportPostStats(PostStats[] models)
		{
			Models = models;
			BuildXmlTree();

			Type resultType = typeof(T);

			if (resultType == typeof(string))
			{
				return (T) Convert.ChangeType(await ReturnXmlContent(), resultType);
			}

			if (resultType == typeof(bool))
			{
				return (T) Convert.ChangeType(await StoreXmlContent(), resultType);
			}

			throw new NotImplementedException("Only string and bool types are supported");
		}

		protected void BuildXmlTree()
		{
			XmlTree.RemoveAll();

			foreach (var model in Models)
			{
				XmlTree.Add(new XElement(ELEMENT_NODE,
					new XElement(ELEMENT_REGION, model.Region),
					new XElement(ELEMENT_QUANTITY, model.Quantity)
				));
			}
		}

		protected async Task<string> ReturnXmlContent()
		{
			using (var memoryStream = new MemoryStream())
			{
				await XmlTree.SaveAsync(memoryStream, SaveOptions.DisableFormatting, new CancellationTokenSource().Token);

				// rewind to the beginning after writing to the stream
				memoryStream.Seek(0, SeekOrigin.Begin);

				int contentSize = (int) memoryStream.Length;
				var byteContent = new byte[contentSize];
				await memoryStream.ReadAsync(byteContent, 0, contentSize);

				// remove BOM from output XML
				return StringUtil.RemoveBom(new UTF8Encoding(false).GetString(byteContent));
			}
		}

		protected async Task<bool> StoreXmlContent()
		{
			return await Task.Run(() => false);
		}
	}
}
