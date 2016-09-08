using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class CollocationSkuInfo : BaseModel
	{
		private long _id;

		public long ColloProductId
		{
			get;
			set;
		}

        public virtual CollocationPoruductInfo ChemCloud_CollocationPoruducts
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

		public decimal Price
		{
			get;
			set;
		}

		public long ProductId
		{
			get;
			set;
		}

		public string SkuID
		{
			get;
			set;
		}

		public decimal? SkuPirce
		{
			get;
			set;
		}

		public CollocationSkuInfo()
		{
		}
	}
}