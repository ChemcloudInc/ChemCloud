using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class ProvinceMode
	{
		public IEnumerable<CityMode> City
		{
			get;
			set;
		}

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

		public ProvinceMode()
		{
		}
	}
}