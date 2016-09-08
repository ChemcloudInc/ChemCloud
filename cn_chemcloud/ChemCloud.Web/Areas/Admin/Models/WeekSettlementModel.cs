using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Admin.Models
{
	public class WeekSettlementModel
	{
		public int CurrentWeekSettlement
		{
			get;
			set;
		}

		public int NewWeekSettlement
		{
			get;
			set;
		}

		public WeekSettlementModel()
		{
		}
	}
}