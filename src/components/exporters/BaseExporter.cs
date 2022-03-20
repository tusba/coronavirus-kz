using System;
using System.Threading.Tasks;
using Tusba.Components.FileSystem;
using Tusba.Models;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Exporters
{
	public abstract class BaseExporter<T> : FileStorage, InterfaceExporter<T> where T : IConvertible
	{
		public virtual async Task<T> ExportPostStats(PostStats[] models)
		{
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

		protected abstract Task<bool> StoreContent();
	}
}
