using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ActiveMarketServiceInfo : BaseModel
	{
		private long _id;

		public new long Id
		{
			get
			{
				return _id;
			}
			set
			{
                _id = value;
				base.Id = value;
			}
		}

		public virtual ICollection<ChemCloud.Model.MarketServiceRecordInfo> MarketServiceRecordInfo
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public string ShopName
		{
			get;
			set;
		}

		public MarketType TypeId
		{
			get;
			set;
		}

		public ActiveMarketServiceInfo()
		{
            MarketServiceRecordInfo = new HashSet<ChemCloud.Model.MarketServiceRecordInfo>();
		}
	}
}