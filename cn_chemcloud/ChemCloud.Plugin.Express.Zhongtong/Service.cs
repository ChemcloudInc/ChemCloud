using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.ExpressPlugin;
using System;
using System.Collections.Generic;

namespace ChemCloud.Plugin.Express.Zhongtong
{
	public class Service : ExpressPluginBase, IExpress, IPlugin
	{
		public Service()
		{
		}

		void ChemCloud.Core.Plugins.IExpress.UpdatePrintElement(IEnumerable<ExpressPrintElement> expressPrintElements)
		{
			base.UpdatePrintElement(expressPrintElements);
		}

		void ChemCloud.Core.Plugins.IPlugin.CheckCanEnable()
		{
			base.CheckCanEnable();
		}

	}
}