using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
	public class CouponController : BaseAdminController
	{
		public CouponController()
		{
		}

		[UnAuthorize]
		public JsonResult List(string shopName, int page, int rows)
		{
			MarketBoughtQuery marketBoughtQuery = new MarketBoughtQuery()
			{
				PageSize = rows,
				PageNo = page,
				ShopName = shopName,
				MarketType = new MarketType?(MarketType.Coupon)
			};
			PageModel<MarketServiceRecordInfo> boughtShopList = ServiceHelper.Create<IMarketService>().GetBoughtShopList(marketBoughtQuery);
			var array = 
				from item in (
                    from m in boughtShopList.Models
                    orderby m.MarketServiceId descending, m.EndTime descending
                    select m).ToArray()
                select new { Id = item.Id, StartDate = item.StartTime.ToString("yyyy-MM-dd"), EndDate = item.EndTime.ToString("yyyy-MM-dd"), ShopName = item.ActiveMarketServiceInfo.ShopName };
			return Json(new { rows = array, total = boughtShopList.Total });
		}

		public ActionResult Management()
		{
			return View();
		}

		[HttpPost]
		[UnAuthorize]
		public JsonResult SaveServiceSetting(decimal Price)
		{
			Result result = new Result();
			MarketSettingInfo marketSettingInfo = new MarketSettingInfo()
			{
				Price = Price,
				TypeId = MarketType.Coupon
			};
			ServiceHelper.Create<IMarketService>().AddOrUpdateServiceSetting(marketSettingInfo);
			result.success = true;
			result.msg = "保存成功！";
			return Json(result);
		}

		public ActionResult ServiceSetting()
		{
			return View(ServiceHelper.Create<IMarketService>().GetServiceSetting(MarketType.Coupon));
		}
	}
}