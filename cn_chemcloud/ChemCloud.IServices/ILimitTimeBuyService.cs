using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface ILimitTimeBuyService : IService, IDisposable
	{
		void AddLimitTimeItem(LimitTimeMarketInfo model);

		void AddServiceCategory(string categoryName);

		void AuditItem(long Id, LimitTimeMarketInfo.LimitTimeMarketAuditStatus status, string message);

		void DeleteServiceCategory(string categoryName);

		void EnableMarketService(int monthCount, long shopId);

		PageModel<ActiveMarketServiceInfo> GetBoughtShopList(MarketBoughtQuery query);

		PageModel<LimitTimeMarketInfo> GetItemList(LimitTimeQuery query);

		LimitTimeMarketInfo GetLimitTimeMarketItem(long id);

		LimitTimeMarketInfo GetLimitTimeMarketItemByProductId(long pid);

		int GetMarketSaleCountForUserId(long pId, long userId);

		ActiveMarketServiceInfo GetMarketService(long shopId);

		string[] GetServiceCategories();

		LimitTimeBuySettingModel GetServiceSetting();

		bool IsLimitTimeMarketItem(long id);

		void UpdateLimitTimeItem(LimitTimeMarketInfo model);

		void UpdateServiceSetting(LimitTimeBuySettingModel model);
	}
}