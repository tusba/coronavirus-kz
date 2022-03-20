using System;
using System.Text;
using System.Threading.Tasks;
using Tusba.Components.FileSystem;
using Tusba.Models;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Exporters
{
	public abstract class BaseExporter<T> : FileStorage, InterfaceExporter<T> where T : IConvertible
	{
		protected Encoding ExportEncoding => new UTF8Encoding(false);
		protected PostStats[] Models { get; set; } = new PostStats[] { };

		public virtual async Task<T> ExportPostStats(PostStats[] models)
		{
			Models = models;
			HookBeforeExport();

			Type resultType = typeof(T);

			if (resultType == typeof(string))
			{
				return (T) Convert.ChangeType(await ReturnContent(), resultType);
			}

			if (resultType == typeof(bool))
			{
				return (T) Convert.ChangeType(await StoreContent(), resultType);
			}

			throw new NotImplementedException("Only string and bool types are supported");
		}

		protected abstract Task<string> ReturnContent();

		protected abstract Task<bool> StoreContentInternally();

		protected virtual void HookBeforeExport()
		{
		}

		protected virtual async Task<bool> StoreContent()
		{
			if (Directory == String.Empty || FileName == String.Empty) {
				return await Task.Run(() => false);
			}

			ProvideDirectory(Directory);

			return await StoreContentInternally();
		}
	}
}
