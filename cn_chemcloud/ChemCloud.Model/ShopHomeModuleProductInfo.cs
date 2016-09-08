using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ShopHomeModuleProductInfo : BaseModel
	{
		private long _id;

		public long HomeModuleId
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

		public long ProductId
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.ProductInfo ProductInfo
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.ShopHomeModuleInfo ShopHomeModuleInfo
		{
			get;
			set;
		}

		public ShopHomeModuleProductInfo()
		{
		}
	}
}