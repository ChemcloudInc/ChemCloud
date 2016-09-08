using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class CapitalDetailQuery : QueryBase
	{
		public CapitalDetailInfo.CapitalDetailType? capitalType
		{
			get;
			set;
		}

		public DateTime? endTime
		{
			get;
			set;
		}

		public long memberId
		{
			get;
			set;
		}

		public DateTime? startTime
		{
			get;
			set;
		}

		public CapitalDetailQuery()
		{
		}
	}
}