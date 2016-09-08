using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class TopicModuleInfo : BaseModel
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

		public virtual ICollection<ChemCloud.Model.ModuleProductInfo> ModuleProductInfo
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public long TopicId
		{
			get;
			set;
		}

		public virtual ChemCloud.Model.TopicInfo TopicInfo
		{
			get;
			set;
		}

		public TopicModuleInfo()
		{
            ModuleProductInfo = new HashSet<ChemCloud.Model.ModuleProductInfo>();
		}
	}
}