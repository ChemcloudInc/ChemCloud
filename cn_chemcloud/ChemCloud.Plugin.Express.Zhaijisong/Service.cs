using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.ExpressPlugin;
using System;
using System.Collections.Generic;

namespace ChemCloud.Plugin.Express.Zhaijisong
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


		public override string NextExpressCode(string currentExpressCode)
		{
			long num = Convert.ToInt64(currentExpressCode) + 11;
			if (num % 10 > 6)
			{
				num = num - 7;
			}
			return num.ToString().PadLeft(currentExpressCode.Length, '0');
		}
	}
}