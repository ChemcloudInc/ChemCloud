using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class CashDepositQuery : QueryBase
	{
		public string ShopName
		{
			get;
			set;
		}

		public bool? Type
		{
			get;
			set;
		}

		public CashDepositQuery()
		{
		}
	}
}