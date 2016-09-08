using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class TypeBrandInfo : BaseModel
	{
		private long _id;

		public long BrandId
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.BrandInfo BrandInfo
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

		public long TypeId
		{
			get;
			set;
		}

		public virtual ProductTypeInfo TypeInfo
		{
			get;
			set;
		}

		public TypeBrandInfo()
		{
		}
	}
}