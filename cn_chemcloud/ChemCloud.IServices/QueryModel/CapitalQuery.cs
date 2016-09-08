using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class CapitalQuery : QueryBase
	{
		public long? memberId
		{
			get;
			set;
		}

		public CapitalQuery()
		{
		}
	}
}