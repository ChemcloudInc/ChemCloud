using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
	public class GiftDetailPageModel
	{
		public bool GiftCanBuy
		{
			get;
			set;
		}

		public GiftInfo GiftData
		{
			get;
			set;
		}

		public List<GiftModel> HotGifts
		{
			get;
			set;
		}

		public GiftDetailPageModel()
		{
		}
	}
}