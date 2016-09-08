using System;

namespace ChemCloud.Core
{
	public class InvalidPropertyException : HimallException
	{
		public InvalidPropertyException()
		{
		}

		public InvalidPropertyException(string message) : base(message)
		{
		}

		public InvalidPropertyException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}