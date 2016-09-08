using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Admin.Models.Product
{
	public class BrandViewModel
	{
		public long id
		{
			get;
			set;
		}

		public bool isChecked
		{
			get;
			set;
		}

		public string @value
		{
			get;
			set;
		}

		public BrandViewModel()
		{
		}
	}
}