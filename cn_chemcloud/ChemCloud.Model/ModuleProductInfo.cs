using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ModuleProductInfo : BaseModel
	{
		private long _id;

		public long DisplaySequence
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

		public long ModuleId
		{
			get;
			set;
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

		public virtual ChemCloud.Model.TopicModuleInfo TopicModuleInfo
		{
			get;
			set;
		}

		public ModuleProductInfo()
		{
		}
	}
}