using System;
using Tusba.Components.Configuration;
using Tusba.Components.Exceptions;
using Tusba.Components.Exporters;
using Tusba.Patterns.FactoryMethod;
using Tusba.Patterns.Visitor.Export;

namespace Tusba.Components.Factories.Post
{
	public class PostStatsExporter<T> : InterfaceFactoryMethod<InterfaceExporter<T>> where T : IConvertible
	{
		public static string Format => Configuration.Get("stats.export.format").ToLower();

		private static InterfaceConfigurationReader Configuration = SystemConfiguration.Instance;

		/**
		 * @throws ApplicationConfigurationException
		 */
		public InterfaceExporter<T> FactoryInstance => Format switch
		{
			"xml" => new XmlExporter<T>(),
			"json" => new JsonExporter<T>(),
			_ => throw new ApplicationConfigurationException("Unrecognized export format")
		};
	}
}
