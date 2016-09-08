using ChemCloud.Core;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class CouponSettingInfo : BaseModel
	{
		public long CouponID
		{
			get;
			set;
		}

		public int? Display
		{
			get;
			set;
		}

        public virtual CouponInfo ChemCloud_Coupon
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public PlatformType PlatForm
		{
			get;
			set;
		}

		public CouponSettingInfo()
		{
		}
	}
}