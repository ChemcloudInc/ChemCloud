using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Mobile.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Mobile.Controllers
{
	public class HomeController : BaseMobileTemplatesController
	{
		public HomeController()
		{
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Index()
		{
			SlideAdInfo.SlideAdType slideAdType;
			slideAdType = (base.PlatformType != ChemCloud.Core.PlatformType.WeiXin ? SlideAdInfo.SlideAdType.WeixinHome : SlideAdInfo.SlideAdType.WeixinHome);
			ChemCloud.Core.PlatformType platformType = ChemCloud.Core.PlatformType.WeiXin;
			IQueryable<SlideAdInfo> slidAds = ServiceHelper.Create<ISlideAdsService>().GetSlidAds(0, slideAdType);
			dynamic viewBag = base.ViewBag;
			SlideAdInfo[] array = slidAds.ToArray();
			viewBag.SlideAds = 
				from item in array
                select new HomeSlideAdsModel()
				{
					ImageUrl = item.ImageUrl,
					Url = item.Url
				};
			MobileHomeTopicsInfo[] mobileHomeTopicsInfoArray = (
				from item in ServiceHelper.Create<IMobileHomeTopicService>().GetMobileHomeTopicInfos(platformType, 0)
				orderby item.Sequence
				select item).ToArray();
			ITopicService topicService = ServiceHelper.Create<ITopicService>();
			IEnumerable<HomeTopicModel> homeTopicModels = mobileHomeTopicsInfoArray.Select<MobileHomeTopicsInfo, HomeTopicModel>((MobileHomeTopicsInfo item) => {
				TopicInfo topicInfo = topicService.GetTopicInfo(item.TopicId);
				string[] strArrays = topicInfo.Tags.Split(new char[] { ',' });
				return new HomeTopicModel()
				{
					ImageUrl = topicInfo.FrontCoverImage,
					Tags = strArrays,
					Id = item.TopicId
				};
			});
			ViewBag.Topics = homeTopicModels;
			IQueryable<MobileHomeProductsInfo> mobileHomeProductsInfos = (
				from item in ServiceHelper.Create<IMobileHomeProductsService>().GetMobileHomePageProducts(0, platformType)
				orderby item.Sequence, item.Id descending
				select item).Take(8);
			IEnumerable<ProductItem> productItems = 
				from item in mobileHomeProductsInfos.ToArray()
                select new ProductItem()
				{
					Id = item.ProductId,
					ImageUrl = item.ChemCloud_Products.GetImage(ProductInfo.ImageSize.Size_350, 1),
					Name = item.ChemCloud_Products.ProductName,
					MarketPrice = item.ChemCloud_Products.MarketPrice,
					SalePrice = item.ChemCloud_Products.MinSalePrice
				};
			ViewBag.Products = productItems;
			ViewBag.SiteName = base.CurrentSiteSetting.SiteName;
			return View();
		}

		public JsonResult LoadProducts(int page, int pageSize)
		{
			IQueryable<MobileHomeProductsInfo> mobileHomeProductsInfos = (
				from item in ServiceHelper.Create<IMobileHomeProductsService>().GetMobileHomePageProducts(0, ChemCloud.Core.PlatformType.WeiXin)
				orderby item.Sequence, item.Id descending
				select item).Skip((page - 1) * pageSize).Take(pageSize);
			var array = 
				from item in mobileHomeProductsInfos.ToArray()
                select new { name = item.ChemCloud_Products.ProductName, id = item.ProductId, image = item.ChemCloud_Products.GetImage(ProductInfo.ImageSize.Size_350, 1), price = item.ChemCloud_Products.MinSalePrice, marketPrice = item.ChemCloud_Products.MarketPrice };
			return Json(array, JsonRequestBehavior.AllowGet);
		}
	}
}