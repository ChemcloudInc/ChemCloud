using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface IMarketService : IService, IDisposable
	{
		void AddOrUpdateServiceSetting(MarketSettingInfo info);

		PageModel<MarketServiceRecordInfo> GetBoughtShopList(MarketBoughtQuery query);

		ActiveMarketServiceInfo GetMarketService(long shopId, MarketType type);

		MarketSettingInfo GetServiceSetting(MarketType type);

		void OrderMarketService(int monthCount, long shopId, MarketType type);
	}
}