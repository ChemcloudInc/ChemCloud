using ChemCloud.Core;
using System;

namespace ChemCloud.ServiceProvider
{
	public class ServiceInstacnceCreateException : HimallException
	{
		public ServiceInstacnceCreateException()
		{
		}

		public ServiceInstacnceCreateException(string message) : base(message)
		{
		}

		public ServiceInstacnceCreateException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}