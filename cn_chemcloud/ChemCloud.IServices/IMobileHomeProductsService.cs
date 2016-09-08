using ChemCloud.Core;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface IMobileHomeProductsService : IService, IDisposable
	{
		void AddProductsToHomePage(long shopId, PlatformType platformType, IEnumerable<long> productIds);

		void Delete(long shopId, long id);

		PageModel<MobileHomeProductsInfo> GetMobileHomePageProducts(long shopId, PlatformType platformType, ProductQuery productQuery);

		IQueryable<MobileHomeProductsInfo> GetMobileHomePageProducts(long shopId, PlatformType platformType);

		PageModel<MobileHomeProductsInfo> GetSellerMobileHomePageProducts(long shopId, PlatformType platformType, ProductQuery productQuery);

		void UpdateSequence(long shopId, long id, short sequenc);
	}
}