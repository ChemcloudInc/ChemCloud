using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ShopHomeModuleInfo : BaseModel
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

		public string Name
		{
			get;
			set;
		}

		public virtual ICollection<ChemCloud.Model.ShopHomeModuleProductInfo> ShopHomeModuleProductInfo
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public ShopHomeModuleInfo()
		{
            ShopHomeModuleProductInfo = new HashSet<ChemCloud.Model.ShopHomeModuleProductInfo>();
		}
	}
}