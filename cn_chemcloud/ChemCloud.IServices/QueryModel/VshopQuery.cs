using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
	public class VshopQuery : QueryBase
	{
		public string Name
		{
			get;
			set;
		}

		public VShopExtendInfo.VShopExtendState VshopState
		{
			get;
			set;
		}

		public VShopExtendInfo.VShopExtendType? VshopType
		{
			get;
			set;
		}

		public VshopQuery()
		{
		}
	}
}