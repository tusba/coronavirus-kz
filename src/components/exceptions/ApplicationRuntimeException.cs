using System;

namespace Tusba.Components.Exceptions
{
	public class ApplicationRuntimeException : ApplicationException
	{
		public ApplicationRuntimeException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
