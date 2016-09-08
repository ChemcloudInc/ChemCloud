using System;

namespace ChemCloud.Core.Plugins
{
	public interface IPlugin
	{
		string WorkDirectory
		{
			set;
		}

		void CheckCanEnable();
	}
}