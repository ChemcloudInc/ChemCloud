using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
	public class ArticleJsonModel
	{
		public string AddDate
		{
			get;
			set;
		}

		public long Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public ArticleJsonModel()
		{
		}
	}
}