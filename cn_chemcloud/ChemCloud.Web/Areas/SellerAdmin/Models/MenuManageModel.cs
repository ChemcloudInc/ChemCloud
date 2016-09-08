using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
	public class MenuManageModel
	{
		public long ID
		{
			get;
			set;
		}

		public MenuInfo.UrlTypes? LinkType
		{
			get;
			set;
		}

		public IEnumerable<MenuInfo> SubMenu
		{
			get;
			set;
		}

		public string TopMenuName
		{
			get;
			set;
		}

		public string URL
		{
			get;
			set;
		}

		public MenuManageModel()
		{
		}
	}
}