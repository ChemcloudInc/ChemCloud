using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class MarketBoughtQuery : QueryBase
	{
		public ChemCloud.Model.MarketType? MarketType
		{
			get;
			set;
		}

		public string ShopName
		{
			get;
			set;
		}

		public MarketBoughtQuery()
		{
		}
	}
}