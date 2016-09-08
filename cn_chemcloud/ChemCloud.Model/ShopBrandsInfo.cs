using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ShopBrandsInfo : BaseModel
	{
		private long _id;

		public long BrandId
		{
			get;
			set;
		}

		public virtual BrandInfo ChemCloud_Brands
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

		public ShopBrandsInfo()
		{
		}
	}
}