using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
	public class AmountModel
	{
		public List<decimal> freightAmounts
		{
			get;
			set;
		}

		public decimal totalAmount
		{
			get;
			set;
		}

		public decimal totalFreightAmount
		{
			get;
			set;
		}

		public decimal totalProductAmount
		{
			get;
			set;
		}

		public AmountModel()
		{
		}
	}
}