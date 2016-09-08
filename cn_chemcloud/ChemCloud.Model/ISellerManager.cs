using System;
using System.Collections.Generic;

namespace ChemCloud.Model
{
	public interface ISellerManager : IManager
	{
		List<SellerPrivilege> SellerPrivileges
		{
			get;
			set;
		}

		long VShopId
		{
			get;
			set;
		}
	}
}