using ChemCloud.Core;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class MobileHomeProductsInfo : BaseModel
	{
		private long _id;

        public virtual ProductInfo ChemCloud_Products
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

		public PlatformType PlatFormType
		{
			get;
			set;
		}

		public long ProductId
		{
			get;
			set;
		}

		public short Sequence
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public MobileHomeProductsInfo()
		{
		}
	}
}