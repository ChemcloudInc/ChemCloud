using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class BaseAdditionalCoupon
	{
		public object Coupon
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public BaseAdditionalCoupon()
		{
		}
	}
}