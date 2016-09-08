using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Web.Models;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using ChemCloud.Model.Common;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
       {
             
        }

        [HttpGet]
        public JsonResult GetFoot()
        {
            IArticleCategoryService articleCategoryService = ServiceHelper.Create<IArticleCategoryService>();
            IArticleService articleService = ServiceHelper.Create<IArticleService>();
            ArticleCategoryInfo specialArticleCategory = articleCategoryService.GetSpecialArticleCategory(SpecialCategory.PageFootService);
            if (specialArticleCategory == null)
            {
                return Json(new List<PageFootServiceModel>(), JsonRequestBehavior.AllowGet);
            }
            IQueryable<ArticleCategoryInfo> articleCategoriesByParentId = articleCategoryService.GetArticleCategoriesByParentId(specialArticleCategory.Id, false);
            IEnumerable<PageFootServiceModel> array =
                from item in articleCategoriesByParentId.ToArray()
                select new PageFootServiceModel()
                {
                    CateogryName = item.Name,
                    Articles =
                        from t in articleService.GetArticleByArticleCategoryId(item.Id)
                        where t.IsRelease
                        select t
                };
            return Json(array, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<string> GetOAuthValidateContents()
        {
            return
                from item in PluginsManagement.GetPlugins<IOAuthPlugin>(true)
                select item.Biz.GetValidateContent();
        }
        [HttpGet]
        public ActionResult ViewLogin()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HomeIndex()
        {
            RedirectToRouteResult redirectToRouteResult = RedirectToAction("index", "home", new { area = "Web" });
            return redirectToRouteResult;
        }
        [HttpPost]
        public JsonResult Login(string password)
        {
            JsonResult jsonResult = Json(new { success = false });

            string pw = "qmsjyff";
            if (password == pw)
            {
                jsonResult = Json(new { success = true });
            }
            return jsonResult;
        }

        [HttpGet]
        public ActionResult Index()
        {

            string str;
            string[] strArrays;

            if (!IsInstalled())
            {
                return RedirectToAction("Agreement", "Installer");
            }

            ServiceHelper.Create<IMemberService>();
            ViewBag.OAuthValidateContents = GetOAuthValidateContents();
            ViewBag.SiteName = base.CurrentSiteSetting.SiteName;
            dynamic viewBag = base.ViewBag;
            strArrays = (!string.IsNullOrWhiteSpace(base.CurrentSiteSetting.Hotkeywords) ? base.CurrentSiteSetting.Hotkeywords.Split(new char[] { ',' }) : new string[0]);
            viewBag.HotKeyWords = strArrays;
            str = (string.IsNullOrWhiteSpace(base.CurrentSiteSetting.Site_SEOTitle) ? "ChemCloud" : base.CurrentSiteSetting.Site_SEOTitle);
            ViewBag.Title = str;
            ViewEngines.Engines.FindView(base.ControllerContext, "Index", null);
            ViewBag.handImage = ServiceHelper.Create<ISlideAdsService>().GetHandSlidAds().ToList();
            List<SlideAdInfo> list = ServiceHelper.Create<ISlideAdsService>().GetSlidAds(0, SlideAdInfo.SlideAdType.PlatformHome).ToList();
            ViewBag.slideImage = list;

            //banner列表（轮播图）
            Result_List<Result_BannersIndex> bannerList = ServiceHelper.Create<IBannersIndexService>().GetBannerList_IndexPage(1);
            ViewBag.Banners = bannerList;
            //交易动态
            Result_List<OrderSynthesis_Index> dynamicTrading = ServiceHelper.Create<IOrderSynthesisService>().GetTopNumOrderSynthesis(26);
            ViewBag.DynamicTrading = dynamicTrading;

            //最近动态报价
            Result_List<ProductInfoSpec> latestPrice = ServiceHelper.Create<IProductService>().Get_Products_Top(30);
            ViewBag.LatestPrice = latestPrice;

            //大家都在忙什么
            Result_List<Result_WhatBusy> whatBusy = ServiceHelper.Create<IWhatBusyService>().Get_WhatBusy_Top(26);
            ViewBag.WhatBusyList = whatBusy;


            //热销产品
            Result_List<OrderSynthesis_Index> HotSelling = ServiceHelper.Create<IOrderSynthesisService>().GetHotSelling();
            ViewBag.HotSelling = HotSelling;


            IEnumerable<ImageAdInfo> imageAds = ServiceHelper.Create<ISlideAdsService>().GetImageAds(0);
            ViewBag.imageAds = imageAds.Where((ImageAdInfo p) =>
            {
                if (p.Id <= 0)
                {
                    return false;
                }
                return p.Id <= 4;
            }).ToList();
            dynamic viewBag1 = base.ViewBag;
            IEnumerable<ImageAdInfo> imageAdInfos = ServiceHelper.Create<ISlideAdsService>().GetImageAds(0);
            ViewBag.imageAdsTop = imageAdInfos.Where((ImageAdInfo p) =>
            {
                if (p.Id == 14)
                {
                    return true;
                }
                return p.Id == 15;
            }).ToList();
            ChemCloud.Service.SiteSettingService a = new ChemCloud.Service.SiteSettingService();
            ViewBag.FirstAmount = int.Parse(a.GetSiteValue("FirstAmount"));
            ViewBag.SecondAmount = int.Parse(a.GetSiteValue("SecondAmount"));
            ViewBag.leijiFirstAmount = int.Parse(a.GetSiteValue("leijiFirstAmount"));
            ViewBag.leijiSecondAmount = int.Parse(a.GetSiteValue("leijiSecondAmount"));
            DateTime Runtime = DateTime.Parse(a.GetSiteValue("Runtime"));
            DateTime Now = DateTime.Now.Date;
            System.TimeSpan ts = Now - Runtime;
            int OperationDays = ts.Days + 1;
            if (OperationDays <= 0)
            {
                OperationDays = 1;
            }
            ViewBag.OperationDays = OperationDays;
            ViewBag.Operationreport = a.GetSiteValue("Operationreport");
            if (string.IsNullOrEmpty(ViewBag.Operationreport)) { ViewBag.Operationreport = "http://47.90.8.156:9100/album/yybg.pdf"; }

            //List<ProductInfo> productInfo = ServiceHelper.Create<IProductService>().RecommendProductList().ToList<ProductInfo>();
            //ViewBag.Products = productInfo;

            List<RecommendInfo> model = ServiceHelper.Create<IRecommendInfoService>().GetRecommendByStatus();
            ViewBag.Products = model;
            int typeid = Convert.ToInt32(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("RecommandIndexImg"));

            List<RecFloorImg> rfiList = ServiceHelper.Create<IRecFloorImgService>().GetAll(4);
            ViewBag.typeid = typeid;
            ViewBag.rfiList = rfiList;
            List<ArticleInfo> articles = ServiceHelper.Create<IArticleService>().GetArticleByArticleCategoryId(21).ToList<ArticleInfo>();
            foreach (ArticleInfo item in articles)
            {
                item.Content = NoHTML(item.Content);
                if (item.Content.Length > 100)
                {
                    item.Content = item.Content.Substring(0, 100) + "..";
                }
            }
            ViewBag.news = articles;



            return View();


        }

        public static string NoHTML(string Htmlstring)
        {
            if (Htmlstring.Trim() == "")
            {
                return "";
            }

            Htmlstring = HttpUtility.HtmlDecode(Htmlstring);
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            while (Htmlstring.IndexOf("'") > 0)
            {
                Htmlstring = Htmlstring.Remove(Htmlstring.IndexOf("'"), 1);
            }
            Htmlstring.Replace("'", "");
            Htmlstring.Replace("\"", "");
            Htmlstring.Replace("-", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring.Replace("查看更多英文别名收起", "");
            Htmlstring.Replace("查看更多中文别名收起", "");
            Htmlstring.Replace("查看更多中文名称收起", "");
            Htmlstring.Replace("查看更多英文名称收起", "");

            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring.Trim());

            return Htmlstring;
        }
        [HttpHead]
        public ContentResult Index(string s)
        {
            return base.Content("");
        }

        public ActionResult Index2()
        {
            return View();
        }

        private bool IsInstalled()
        {
            string item = ConfigurationManager.AppSettings["IsInstalled"];
            if (item == null)
            {
                return true;
            }
            return bool.Parse(item);
        }

        public ActionResult TestLogin()
        {
            return View();
        }

        /// <summary>
        /// 首页询价产品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult PageList_Products(int page, int rows, string productname)
        {
            ProductQuery productquery = new ProductQuery() { PageSize = rows, PageNo = page, ProductName = productname };
            PageModel<ProductInfo> products = ServiceHelper.Create<IProductService>().GetProducts(productquery);

            IEnumerable<ProductInfo> array = from item in products.Models.ToArray()
                                             select new ProductInfo
                                                  {
                                                      Id = item.Id,
                                                      ProductCode = item.ProductCode,
                                                      ProductName = item.ProductName,
                                                      EProductName = item.EProductName,
                                                      ShopId = item.ShopId,
                                                      ShopName = (ServiceHelper.Create<IShopService>().GetShopName(item.ShopId) == null ? "" : ServiceHelper.Create<IShopService>().GetShopName(item.ShopId)),
                                                      ShortDescription = item.ShortDescription,
                                                      MarketPrice = item.MarketPrice,
                                                      Purity = item.Purity,
                                                      CASNo = item.CASNo,
                                                      HSCODE = item.HSCODE,
                                                      DangerLevel = item.DangerLevel,
                                                      MolecularFormula = item.MolecularFormula,
                                                      ISCASNo = item.ISCASNo,
                                                      EditStatus = item.EditStatus,
                                                      MeasureUnit = item.MeasureUnit,
                                                      Quantity = item.Quantity,
                                                      Volume = item.Volume,
                                                      Weight = item.Weight
                                                  };
            DataGridModel<ProductInfo> dataGridModel = new DataGridModel<ProductInfo>()
            {
                rows = array,
                total = products.Total
            };
            return Json(dataGridModel);
        }


        /// <summary>
        /// 加载首页实时交易——左侧折线图
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string Get_TransactionRecord_List()
        {
            Result_List1<Result_TransactionRecord> res = ServiceHelper.Create<ITransactionRecordService>().GetTransactionRecordList_Web(6);
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }


    }
}