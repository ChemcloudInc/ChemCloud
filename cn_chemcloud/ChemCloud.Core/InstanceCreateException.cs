using System;

namespace ChemCloud.Core
{
	public class InstanceCreateException : HimallException
	{
		public InstanceCreateException()
		{
		}

		public InstanceCreateException(string message) : base(message)
		{
		}

		public InstanceCreateException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}