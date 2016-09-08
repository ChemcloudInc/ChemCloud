using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class SellerSpecificationValueInfo : BaseModel
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

		public long ShopId
		{
			get;
			set;
		}

		public SpecificationType Specification
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.SpecificationValueInfo SpecificationValueInfo
		{
			get;
			set;
		}

		public long TypeId
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public long ValueId
		{
			get;
			set;
		}

		public SellerSpecificationValueInfo()
		{
		}
	}
}