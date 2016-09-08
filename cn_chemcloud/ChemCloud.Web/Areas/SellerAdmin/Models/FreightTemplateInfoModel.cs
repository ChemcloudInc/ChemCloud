using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
	public class FreightTemplateInfoModel
	{
		public long Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public long ShopID
		{
			get;
			set;
		}

		public FreightTemplateInfoModel()
		{
		}
	}
}