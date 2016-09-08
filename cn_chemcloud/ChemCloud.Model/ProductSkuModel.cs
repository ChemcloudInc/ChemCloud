using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ProductSkuModel
	{
		public string ImgUrl
		{
			get;
			set;
		}

		public long productId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public List<SKUModel> SKUs
		{
			get;
			set;
		}

		public ProductSkuModel()
		{
		}
	}
}