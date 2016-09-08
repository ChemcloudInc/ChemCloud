using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.Model;
using System;
using System.Collections.Generic;

namespace ChemCloud.IServices
{
	public interface IExpressService : IService, IDisposable
	{
		IEnumerable<IExpress> GetAllExpress();

		IExpress GetExpress(string name);

		ExpressData GetExpressData(string expressCompanyName, string shipOrderNumber);

		IDictionary<int, string> GetPrintElementIndexAndOrderValue(long shopId, long orderId, IEnumerable<int> printElementIndexes);

		IEnumerable<IExpress> GetRecentExpress(long shopId, int takeNumber);

		void UpdatePrintElement(string name, IEnumerable<ExpressPrintElement> elements);
	}
}