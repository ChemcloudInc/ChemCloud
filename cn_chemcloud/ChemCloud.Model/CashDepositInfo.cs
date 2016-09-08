using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class CashDepositInfo : BaseModel
	{
		private long _id;

		public decimal CurrentBalance
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public bool EnableLabels
		{
			get;
			set;
		}

        public virtual ICollection<CashDepositDetailInfo> ChemCloud_CashDepositDetail
		{
			get;
			set;
		}

        public virtual ShopInfo ChemCloud_Shops
		{
			get;
			set;
		}

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

		public long ShopId
		{
			get;
			set;
		}

		public decimal TotalBalance
		{
			get;
			set;
		}

		public CashDepositInfo()
		{
            ChemCloud_CashDepositDetail = new HashSet<CashDepositDetailInfo>();
		}
	}
}