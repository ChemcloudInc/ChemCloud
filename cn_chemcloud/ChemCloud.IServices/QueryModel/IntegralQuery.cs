using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class IntegralQuery : QueryBase
	{
		public DateTime? EndDate
		{
			get;
			set;
		}

		public DateTime? StartDate
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public IntegralQuery()
		{
		}
	}
}