using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ShopBonusGrantInfo : BaseModel
	{
		private long _id;

		public string BonusQR
		{
			get;
			set;
		}

        public virtual UserMemberInfo ChemCloud_Members
		{
			get;
			set;
		}

		public virtual ShopBonusInfo ChemCloud_ShopBonus
		{
			get;
			set;
		}

		public virtual ICollection<ShopBonusReceiveInfo> ChemCloud_ShopBonusReceive
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

		public long OrderId
		{
			get;
			set;
		}

		public long ShopBonusId
		{
			get;
			set;
		}

		public long UserId
		{
			get;
			set;
		}

		public ShopBonusGrantInfo()
		{
            ChemCloud_ShopBonusReceive = new HashSet<ShopBonusReceiveInfo>();
		}
	}
}