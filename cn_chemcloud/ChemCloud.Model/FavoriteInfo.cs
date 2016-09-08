using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class FavoriteInfo : BaseModel
	{
		private long _id;

		public DateTime Date
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

		public virtual UserMemberInfo MemberInfo
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

		public string Tags
		{
			get;
			set;
		}

		public long UserId
		{
			get;
			set;
		}

		public FavoriteInfo()
		{
		}
	}
}