using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Core.Plugins
{
	public class Plugin<T> : PluginBase
	where T : IPlugin
	{
		public T Biz
		{
			get;
			set;
		}

		public Plugin()
		{
		}
	}
}