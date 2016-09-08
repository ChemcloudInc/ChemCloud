using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
	public class WebFloorProduct
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

		public string Pic
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}

		public WebFloorProduct()
		{
		}
	}
}