using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
	public class OrderModel
	{
		public int orderCount
		{
			get;
			set;
		}

		public ProductInfo productInfo
		{
			get;
			set;
		}

		public OrderModel()
		{
		}
	}
}