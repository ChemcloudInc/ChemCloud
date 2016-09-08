using System;

namespace ChemCloud.Core
{
	public class RegistAtStart
	{
		public RegistAtStart()
		{
		}

		public static void Regist()
		{
			PluginsManagement.RegistAtStart();
		}
	}
}