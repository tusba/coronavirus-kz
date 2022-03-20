using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using StreamUtil = Tusba.Components.Util.Stream;
using StringUtil = Tusba.Components.Util.String;

namespace Tusba.Components.Exporters
{
	public class XmlExporter<T> : BaseExporter<T> where T : IConvertible
	{
		private const string ELEMENT_ROOT = "PostStats";
		private const string ELEMENT_NODE = "Entry";
		private const string ELEMENT_REGION = "Region";
		private const string ELEMENT_QUANTITY = "Quantity";

		protected XElement XmlTree { get; set; } = new XElement(ELEMENT_ROOT);

		private CancellationTokenSource AsyncCancellation = new CancellationTokenSource();
		private (SaveOptions Options, CancellationToken Token) StreamSaveAsyncOptions => (
			Options: SaveOptions.DisableFormatting,
			Token: AsyncCancellation.Token
		);
		private XmlWriterSettings XmlWriterOptions => new XmlWriterSettings
		{
			Async = true,
			Encoding = ExportEncoding,
			Indent = false
		};

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

		protected override void HookBeforeExport()
		{
			base.HookBeforeExport();
			BuildXmlTree();
		}

		protected override async Task<string> ReturnContent()
		{
			using (var memoryStream = new MemoryStream())
			{
				var (Options, Token) = StreamSaveAsyncOptions;
				await XmlTree.SaveAsync(memoryStream, Options, Token);

				// remove BOM from output XML
				return StringUtil.RemoveBom(await StreamUtil.Read(memoryStream, ExportEncoding));
			}
		}

		protected override async Task<bool> StoreContentInternally()
		{
			using (var xmlWriter = XmlWriter.Create(FileName, XmlWriterOptions))
			{
				try
				{
					await XmlTree.SaveAsync(xmlWriter, StreamSaveAsyncOptions.Token);

					return true;
				}
				catch
				{
					return false;
				}
			}
		}
	}
}
