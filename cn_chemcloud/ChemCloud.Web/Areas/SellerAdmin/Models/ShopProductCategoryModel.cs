using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
	public class ShopProductCategoryModel
	{
		public List<CategoryJsonModel> Data
		{
			get;
			set;
		}

		public List<ChemCloud.Web.Areas.SellerAdmin.Models.SelectedCategory> SelectedCategory
		{
			get;
			set;
		}

		public ShopProductCategoryModel()
		{
		}
	}
}