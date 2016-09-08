using ChemCloud.Core;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class MobileHomeTopicsInfo : BaseModel
	{
		private long _id;

        public virtual TopicInfo ChemCloud_Topics
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

		public PlatformType Platform
		{
			get;
			set;
		}

		public int Sequence
		{
			get;
			set;
		}

		public long ShopId
		{
			get;
			set;
		}

		public long TopicId
		{
			get;
			set;
		}

		public MobileHomeTopicsInfo()
		{
		}
	}
}