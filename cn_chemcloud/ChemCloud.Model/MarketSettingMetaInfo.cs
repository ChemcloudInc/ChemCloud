using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class MarketSettingMetaInfo : BaseModel
	{
		private int _id;

        public virtual MarketSettingInfo ChemCloud_MarketSetting
		{
			get;
			set;
		}

		public new int Id
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

		public int MarketId
		{
			get;
			set;
		}

		public string MetaKey
		{
			get;
			set;
		}

		public string MetaValue
		{
			get;
			set;
		}

		public MarketSettingMetaInfo()
		{
		}
	}
}