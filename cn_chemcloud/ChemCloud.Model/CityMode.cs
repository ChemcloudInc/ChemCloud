using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	public class CityMode
	{
		public IEnumerable<CountyMode> County
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

		public CityMode()
		{
		}
	}
}