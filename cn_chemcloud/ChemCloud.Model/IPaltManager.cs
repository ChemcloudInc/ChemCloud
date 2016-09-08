using System;
using System.Collections.Generic;

namespace ChemCloud.Model
{
	public interface IPaltManager : IManager
	{
		List<AdminPrivilege> AdminPrivileges
		{
			get;
			set;
		}
	}
}