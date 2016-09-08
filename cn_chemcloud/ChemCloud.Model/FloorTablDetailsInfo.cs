using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class FloorTablDetailsInfo : BaseModel
	{
		private long _id;

        public virtual FloorTablsInfo ChemCloud_FloorTabls
		{
			get;
			set;
		}

        public virtual ProductInfo ChemCloud_Products
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

		public long TabId
		{
			get;
			set;
		}

		public FloorTablDetailsInfo()
		{
		}
	}
}