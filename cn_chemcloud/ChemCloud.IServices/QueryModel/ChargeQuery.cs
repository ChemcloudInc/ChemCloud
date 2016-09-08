using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class ChargeQuery : QueryBase
	{
		public long? ChargeNo
		{
			get;
			set;
		}

		public ChargeDetailInfo.ChargeDetailStatus? ChargeStatus
		{
			get;
			set;
		}

		public long? memberId
		{
			get;
			set;
		}

		public ChargeQuery()
		{
		}
	}
}