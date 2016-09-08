using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Core.Plugins
{
	public abstract class PluginBase
	{
		public ChemCloud.Core.Plugins.PluginInfo PluginInfo
		{
			get;
			set;
		}

		protected PluginBase()
		{
		}
	}
}