using System;

namespace ChemCloud.Web.Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class NoCompress : Attribute
	{
		public NoCompress()
		{
		}
	}
}