using System.Linq;
using System.Web;
using ChemCloud.Core.Helper;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.Web.Areas.Web.Models;
using com.paypal.sdk.util;
using System.Configuration;
using ChemCloud.Web.Controllers;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class StatisticsController : BaseSellerController
    {
        public StatisticsController()
        {
        }

        public ActionResult DealConversionRate()
        {
            ViewBag.YearDrop = GetYearDrop(2014, 2024);
            ViewBag.MonthDrop = GetMonthDrop();
            IStatisticsService statisticsService = ServiceHelper.Create<IStatisticsService>();
            long shopId = base.CurrentSellerManager.ShopId;
            int year = DateTime.Now.Year;
            DateTime now = DateTime.Now;
            LineChartDataModel<float> dealConversionRateChart = statisticsService.GetDealConversionRateChart(shopId, year, now.Month);
            return View(new ChartDataViewModel(dealConversionRateChart));
        }

        [HttpGet]
        [UnAuthorize]
        public JsonResult GetDealConversionRateChartByMonth(int year = 0, int month = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            LineChartDataModel<float> dealConversionRateChart = ServiceHelper.Create<IStatisticsService>().GetDealConversionRateChart(base.CurrentSellerManager.ShopId, year, month);
            return Json(new { successful = true, chart = dealConversionRateChart }, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetMonthDrop()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            for (int i = 1; i < 13; i++)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Selected = DateTime.Now.Month == i,
                    Text = i.ToString(),
                    Value = i.ToString()
                };
                selectListItems.Add(selectListItem);
            }
            return selectListItems;
        }

        [HttpGet]
        [UnAuthorize]
        public JsonResult GetSaleRankingChart(string day = "", int year = 0, int month = 0, int weekIndex = 0, int dimension = 1)
        {
            DateTime now;
            LineChartDataModel<int> lineChartDataModel = new LineChartDataModel<int>();
            if (string.IsNullOrWhiteSpace(day))
            {
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                lineChartDataModel = (weekIndex != 0 ? ServiceHelper.Create<IStatisticsService>().GetProductSaleRankingChart(base.CurrentSellerManager.ShopId, year, month, weekIndex, (SaleDimension)dimension, 15) : ServiceHelper.Create<IStatisticsService>().GetProductSaleRankingChart(base.CurrentSellerManager.ShopId, year, month, (SaleDimension)dimension, 15));
            }
            else
            {
                if (!DateTime.TryParse(day, out now))
                {
                    now = DateTime.Now;
                }
                lineChartDataModel = ServiceHelper.Create<IStatisticsService>().GetProductSaleRankingChart(base.CurrentSellerManager.ShopId, now, (SaleDimension)dimension, 15);
            }
            return Json(new { successful = true, chart = lineChartDataModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [UnAuthorize]
        public JsonResult GetShopFlowChartByMonth(int year = 0, int month = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            LineChartDataModel<int> shopFlowChart = ServiceHelper.Create<IStatisticsService>().GetShopFlowChart(base.CurrentSellerManager.ShopId, year, month);
            return Json(new { successful = true, chart = shopFlowChart }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [UnAuthorize]
        public JsonResult GetShopSaleChartByMonth(int year = 0, int month = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            LineChartDataModel<int> shopSaleChart = ServiceHelper.Create<IStatisticsService>().GetShopSaleChart(base.CurrentSellerManager.ShopId, year, month);
            return Json(new { successful = true, chart = shopSaleChart }, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetWeekDrop(int year, int month)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            DateTime startDayOfWeeks = DateTimeHelper.GetStartDayOfWeeks(year, month, 1);
            for (int i = 1; i <= 4; i++)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Selected = i == 1
                };
                string str = startDayOfWeeks.ToString("yyyy-MM-dd");
                DateTime dateTime = startDayOfWeeks.AddDays(6);
                selectListItem.Text = string.Format("{0} -- {1}", str, dateTime.ToString("yyyy-MM-dd"));
                selectListItem.Value = i.ToString();
                selectListItems.Add(selectListItem);
                startDayOfWeeks = startDayOfWeeks.AddDays(7);
            }
            return selectListItems;
        }

        private List<SelectListItem> GetYearDrop(int start, int end)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            for (int i = start; i < end; i++)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Selected = DateTime.Now.Year == i,
                    Text = i.ToString(),
                    Value = i.ToString()
                };
                selectListItems.Add(selectListItem);
            }
            return selectListItems;
        }

        [HttpGet]
        [UnAuthorize]
        public ActionResult ProductSaleRanking()
        {
            ViewBag.YearDrop = GetYearDrop(2014, 2024);
            ViewBag.MonthDrop = GetMonthDrop();
            dynamic viewBag = base.ViewBag;
            int year = DateTime.Now.Year;
            DateTime now = DateTime.Now;
            viewBag.WeekDrop = GetWeekDrop(year, now.Month);
            return View();
        }

        [HttpGet]
        [UnAuthorize]
        public ActionResult ProductVisitRanking()
        {
            ViewBag.YearDrop = GetYearDrop(2014, 2024);
            ViewBag.MonthDrop = GetMonthDrop();
            dynamic viewBag = base.ViewBag;
            int year = DateTime.Now.Year;
            DateTime now = DateTime.Now;
            viewBag.WeekDrop = GetWeekDrop(year, now.Month);
            return View();
        }

        [HttpGet]
        [UnAuthorize]
        public JsonResult ProductVisitRankingChart(string day = "", int year = 0, int month = 0, int weekIndex = 0)
        {
            DateTime now;
            LineChartDataModel<int> lineChartDataModel = new LineChartDataModel<int>();
            if (string.IsNullOrWhiteSpace(day))
            {
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                lineChartDataModel = (weekIndex != 0 ? ServiceHelper.Create<IStatisticsService>().GetProductVisitRankingChart(base.CurrentSellerManager.ShopId, year, month, weekIndex, 15) : ServiceHelper.Create<IStatisticsService>().GetProductVisitRankingChart(base.CurrentSellerManager.ShopId, year, month, 15));
            }
            else
            {
                if (!DateTime.TryParse(day, out now))
                {
                    now = DateTime.Now;
                }
                lineChartDataModel = ServiceHelper.Create<IStatisticsService>().GetProductVisitRankingChart(base.CurrentSellerManager.ShopId, now, 15);
            }
            return Json(new { successful = true, chart = lineChartDataModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShopFlow()
        {
            ViewBag.YearDrop = GetYearDrop(2014, 2024);
            ViewBag.MonthDrop = GetMonthDrop();
            IStatisticsService statisticsService = ServiceHelper.Create<IStatisticsService>();
            long shopId = base.CurrentSellerManager.ShopId;
            int year = DateTime.Now.Year;
            DateTime now = DateTime.Now;
            LineChartDataModel<int> shopFlowChart = statisticsService.GetShopFlowChart(shopId, year, now.Month);
            return View(new ChartDataViewModel(shopFlowChart));
        }

        public ActionResult ShopSale()
        {
            ViewBag.YearDrop = GetYearDrop(2014, 2024);
            ViewBag.MonthDrop = GetMonthDrop();
            IStatisticsService statisticsService = ServiceHelper.Create<IStatisticsService>();
            long shopId = base.CurrentSellerManager.ShopId;
            int year = DateTime.Now.Year;
            DateTime now = DateTime.Now;
            LineChartDataModel<int> shopSaleChart = statisticsService.GetShopSaleChart(shopId, year, now.Month);
            return View(new ChartDataViewModel(shopSaleChart));
        }

        public ActionResult MoneyList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(int page, int rows, string starttime, string endtime)
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            StatisticsQuery sm = new StatisticsQuery()
            {
                beginTime = starttime,
                endTime = endtime,
                userid = base.CurrentUser.Id,
                PageNo = page,
                PageSize = rows,
                shopId = base.CurrentSellerManager.ShopId
            };
            PageModel<OrderMoneyModel> pm = isms.GetList1(sm);
            IEnumerable<OrderMoneyModel> models =
            from item in pm.Models.ToArray()
            select new OrderMoneyModel()
            {
                Id = item.Id,
                UserId = item.UserId,
                UserName = item.UserName,
                OrderDate = item.OrderDate,
                TradingPrice = item.TradingPrice
            };
            DataGridModel<OrderMoneyModel> dataGridModel = new DataGridModel<OrderMoneyModel>()
            {
                rows = models,
                total = pm.Total
            };
            return Json(dataGridModel);
        }

        [HttpPost]
        public string GetMoney(string starttime, string endtime)
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            StatisticsQuery sm = new StatisticsQuery()
            {
                beginTime = starttime,
                endTime = endtime,
                shopId = base.CurrentSellerManager.ShopId
            };
            decimal addMoney = isms.GetMoney(sm);
            return addMoney.ToString();
        }
        [HttpPost]
        public string GetMyMoney()
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            decimal myMoney = isms.getMyMoney(base.CurrentSellerManager.Id, 2);
            return myMoney.ToString();
        }

        public ActionResult SticsMoneyList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetManagersAccountList(int page, int rows, string starttime, string endtime)
        {
            IManagersAccountService IMAS = ServiceHelper.Create<IManagersAccountService>();
            ManagersAccountQuery MQ = new ManagersAccountQuery()
            {
                startTime = starttime,
                endTime = endtime,
                PageNo = page,
                PageSize = rows,
                managerId = base.CurrentSellerManager.ShopId
            };
            PageModel<ManagersAccount> PM = IMAS.GetManagersAccountList(MQ);
            IEnumerable<ManagersAccount> models =
            from item in PM.Models.ToArray()
            select new ManagersAccount()
            {
                Id = item.Id,
                Zhuanzhang = item.Zhuanzhang,
                ZhuanzhangName = item.ZhuanzhangName,
                Tiqu = item.Tiqu,
                Tiqufeiyong = item.Tiqufeiyong,
                Tiquhuobi = item.Tiquhuobi,
                Huilv = item.Huilv,
                Tuikuan = item.Tuikuan,
                OrderNum = item.OrderNum,
                Balance = item.Balance,
                BalanceAvailable = item.BalanceAvailable,
                Datatime = item.Datatime,
                ManagerId = item.ManagerId,
                Daikuan = item.Daikuan
            };
            DataGridModel<ManagersAccount> dataGridModel = new DataGridModel<ManagersAccount>()
            {
                rows = models,
                total = PM.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult CapitalCharge()
        {
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            CapitalInfo capitalInfo = memberCapitalService.GetCapitalInfoByManagerId(base.CurrentSellerManager.Id);
            if (capitalInfo == null)
            {
                capitalInfo = new CapitalInfo()
                {
                    MemId = 0,
                    Balance = 0,
                    FreezeAmount = 0,
                    ChargeAmount = 0,
                    ManageId = base.CurrentSellerManager.Id
                };
                memberCapitalService.AddCapitalInfo(capitalInfo);
            }
            return View(capitalInfo);
        }

        public ActionResult ApplyWithDraw()
        {
            SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            //if (string.IsNullOrWhiteSpace(siteSettings.WeixinAppId) || string.IsNullOrWhiteSpace(siteSettings.WeixinAppSecret))
            //{
            //    throw new HimallException("未配置公众号参数");
            //}
            ////string str = "";//AccessTokenContainer.TryGetToken(siteSettings.WeixinAppId, siteSettings.WeixinAppSecret, false);
            //SceneModel sceneModel = new SceneModel(QR_SCENE_Type.WithDraw)
            //{
            //    Object = base.CurrentSellerManager.Id.ToString()
            //};
            int num = 0;//(new SceneHelper()).SetModel(sceneModel, 600);
            //CreateQrCodeResult createQrCodeResult = QrCodeApi.Create(str, 300, num, 10000);
            ViewBag.ticket = "";//createQrCodeResult.ticket;
            ViewBag.Sceneid = num;
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            CapitalInfo capitalInfo = memberCapitalService.GetCapitalInfoByManagerId(base.CurrentSellerManager.Id);
            if (capitalInfo == null)
            {
                ViewBag.ApplyWithMoney = 0;
            }
            else
            {
                dynamic viewBag = base.ViewBag;
                decimal? balance = capitalInfo.Balance;
                viewBag.ApplyWithMoney = balance.Value;
            }
            base.ViewBag.IsSetPwd = (string.IsNullOrWhiteSpace(base.CurrentSellerManager.Password) ? false : true);
            return View();
        }
        [HttpPost]
        public JsonResult ApplyWithDrawSubmit(string openid, string nickname, decimal amount, string pwd)
        {
            if (ServiceHelper.Create<IManagerService>().GetManageInfoByPwd(base.CurrentSellerManager.Id, pwd) == null)
            {
                throw new HimallException("登录密码不对，请重新输入！");
            }
            CapitalInfo capitalInfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfoByManagerId(base.CurrentSellerManager.Id);
            if (capitalInfo == null)
            {
                CapitalInfo cinfo = new CapitalInfo
                {
                    MemId = 0,
                    Balance = 0,
                    FreezeAmount = 0,
                    ChargeAmount = 0,
                    ManageId = base.CurrentSellerManager.Id
                };
                ServiceHelper.Create<IMemberCapitalService>().AddCapitalInfo(cinfo);
                throw new HimallException("您的可用余额不足！");
            }
            else
            {
                decimal num = amount;
                decimal? balance = capitalInfo.Balance;
                if ((num <= balance.GetValueOrDefault() ? false : balance.HasValue))
                {
                    throw new HimallException("提现金额不能超出可用金额！");
                }
                decimal? newvalue = capitalInfo.Balance - num;
                if (newvalue >= 0)
                {
                    ServiceHelper.Create<IMemberCapitalService>().UpdateCapitalAmount(capitalInfo.Id, 0, newvalue, 0, 0, base.CurrentSellerManager.Id);
                }
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult AddStatisticsMoney(int TradingType, decimal TradingPrice, string OrderNum, int PayType, string TypeID)
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            decimal moneybynow = isms.GetMoneyByUidType(base.CurrentSellerManager.Id, 2);
            StatisticsMoney entity = new StatisticsMoney
            {
                UserId = base.CurrentSellerManager.Id,
                UserName = base.CurrentSellerManager.UserName,
                UserType = 2,
                TradingTime = DateTime.Now,
                TradingType = TradingType,
                TradingPrice = TradingPrice,
                OrderNum = OrderNum,
                PayType = PayType,
                Balance = moneybynow,
                BalanceAble = moneybynow
            };
            isms.Add(entity);
            //CapitalInfo cinfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfo(base.CurrentSellerManager.Id);

            return Json(new { Successful = true });
        }

        public JsonResult AddStatisticsMoneyLog(int TradingType, decimal TradingPrice, string OrderNum, int PayType, string TypeID)
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            decimal moneybynow = isms.GetMoneyByUidType(base.CurrentSellerManager.Id, 2);
            StatisticsMoney entity = new StatisticsMoney
            {
                UserId = base.CurrentSellerManager.Id,
                UserName = base.CurrentSellerManager.UserName,
                UserType = 2,
                TradingTime = DateTime.Now,
                TradingType = TradingType,
                TradingPrice = TradingPrice,
                OrderNum = OrderNum,
                PayType = PayType,
                Balance = moneybynow + TradingPrice,
                BalanceAble = moneybynow + TradingPrice
            };
            isms.Add(entity);
            CapitalInfo cinfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfoByManagerId(base.CurrentSellerManager.Id);
            if (cinfo != null)
            {
                cinfo.Balance += TradingPrice;
                ServiceHelper.Create<IMemberCapitalService>().UpdateCapitalAmount(cinfo.Id, 0, cinfo.Balance, 0, 0, base.CurrentSellerManager.Id);
            }
            return Json(new { Successful = true });
        }
        [HttpPost]
        public JsonResult ChargeSubmit(decimal amount)
        {
            ChargeDetailInfo chargeDetailInfo = new ChargeDetailInfo()
            {
                ChargeAmount = amount,
                ChargeStatus = ChargeDetailInfo.ChargeDetailStatus.WaitPay,
                CreateTime = new DateTime?(DateTime.Now),
                MemId = base.CurrentSellerManager.Id
            };
            long num = ServiceHelper.Create<IMemberCapitalService>().AddChargeApply(chargeDetailInfo);
            return Json(new { success = true, msg = num.ToString() });
        }
        [HttpPost]
        public JsonResult GetPayPalId(string pp)
        {
            string strReutrn = "";
            ICapitalUserAccountService ICMAS = ServiceHelper.Create<ICapitalUserAccountService>();
            CapitalUserAccount CMA = ICMAS.GetICapitalUserAccountInfo(base.CurrentUser.Id);
            CapitalUserAccount cm;
            if (CMA == null)
            {
                if (!string.IsNullOrEmpty(pp))
                {
                    cm = new CapitalUserAccount()
                    {
                        userid = base.CurrentSellerManager.Id,
                        CashAccount = pp,
                        CashAccountType = 0,
                        userType = 2
                    };
                    strReutrn = cm.CashAccount;
                    ICMAS.AddICapitalUserAccount(cm);
                }
            }
            else
            {
                CMA.CashAccount = pp;
                ICMAS.UPCapitalUserAccount(CMA);
                strReutrn = CMA.CashAccount;
            }
            return Json(new { result = strReutrn });
        }

        public ActionResult ChargePay(string orderIds)
        {
            string str;
            if (string.IsNullOrEmpty(orderIds))
            {
                return RedirectToAction("/SellerAdmin");
            }
            IMemberCapitalService memberCapitalService = ServiceHelper.Create<IMemberCapitalService>();
            ChargeDetailInfo chargeDetail = memberCapitalService.GetChargeDetail(long.Parse(orderIds));
            if (chargeDetail == null || chargeDetail.MemId != base.CurrentSellerManager.Id || chargeDetail.ChargeStatus == ChargeDetailInfo.ChargeDetailStatus.ChargeSuccess)
            {
                Log.Error(string.Concat("未找到充值申请记录：", orderIds));
                return RedirectToAction("SticsMoneyList");
            }
            SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            ViewBag.Logo = siteSettings.Logo;
            ViewBag.Orders = chargeDetail;
            string scheme = base.Request.Url.Scheme;
            string host = base.HttpContext.Request.Url.Host;
            if (base.HttpContext.Request.Url.Port == 80)
            {
                str = "";
            }
            else
            {
                int port = base.HttpContext.Request.Url.Port;
                str = string.Concat(":", port.ToString());
            }
            string str1 = string.Concat(scheme, "://", host, str);
            string str2 = string.Concat(str1, "/Pay/CapitalChargeReturn/{0}");
            string str3 = string.Concat(str1, "/Pay/CapitalChargeNotify/{0}");
            IEnumerable<Plugin<IPaymentPlugin>> plugins =
                from item in PluginsManagement.GetPlugins<IPaymentPlugin>(true)
                where item.Biz.SupportPlatforms.Contains<PlatformType>(PlatformType.PC)
                select item;
            IEnumerable<PaymentModel> paymentModels = plugins.Select<Plugin<IPaymentPlugin>, PaymentModel>((Plugin<IPaymentPlugin> item) =>
            {
                string empty = string.Empty;
                try
                {
                    empty = item.Biz.GetRequestUrl(string.Format(str2, EncodePaymentId(item.PluginInfo.PluginId)), string.Format(str3, EncodePaymentId(item.PluginInfo.PluginId)), orderIds, chargeDetail.ChargeAmount, "预付款充值", null);
                }
                catch (Exception exception)
                {
                    Log.Error("支付页面加载支付插件出错", exception);
                }
                return new PaymentModel()
                {
                    Logo = string.Concat("/Plugins/Payment/", item.PluginInfo.ClassFullName.Split(new char[] { ',' })[1], "/", item.Biz.Logo),
                    RequestUrl = empty,
                    UrlType = item.Biz.RequestUrlType,
                    Id = item.PluginInfo.PluginId
                };
            });
            paymentModels =
                from item in paymentModels
                where !string.IsNullOrEmpty(item.RequestUrl)
                select item;
            ViewBag.OrderIds = orderIds;
            ViewBag.TotalAmount = chargeDetail.ChargeAmount;
            ViewBag.Step = 1;
            ViewBag.UnpaidTimeout = siteSettings.UnpaidTimeout;
            return View();
        }

        private string EncodePaymentId(string paymentId)
        {
            return paymentId.Replace(".", "-");
        }
        [HttpPost]
        public JsonResult UpOrderStatus(long orderIds,int orderstatus)
        {
            ServiceHelper.Create<IOrderService>().UpdateOrderStatu(orderIds,2);
            return Json(new { Successful = true });
        }
        [HttpPost]
        public JsonResult AddAccount(string ordernum, string orderprice)
        {
            IStatisticsMoneyService imcs = ServiceHelper.Create<IStatisticsMoneyService>();
            decimal moneynow = imcs.GetMoneyByUidType(base.CurrentSellerManager.Id, 2);
            IManagersAccountService IMAS = ServiceHelper.Create<IManagersAccountService>();
            decimal price;
            if (decimal.TryParse(orderprice, out price))
            {
                price = decimal.Parse(orderprice);
            }
            ManagersAccount MA = new ManagersAccount()
            {
                Zhuanzhang = 0,
                ZhuanzhangName = "",
                Tiqu = price,
                Tiqufeiyong = 0,
                Tiquhuobi = "1",
                Huilv = "",
                Tuikuan = 0,
                OrderNum = ordernum,
                Balance = moneynow,
                BalanceAvailable = moneynow,
                Datatime = DateTime.Now,
                ManagerId = base.CurrentSellerManager.Id,
                Daikuan = 0
            };
            IMAS.AddManagersAccountInfo(MA);
            return Json(new { Successful = true });
        }

        public JsonResult UpChargeOrderStatus(long orderIds)
        {
            ServiceHelper.Create<IMemberCapitalService>().UpdateChareeOrderStatu(orderIds);
            return Json(new { Successful = true });
        }        

        public ActionResult Return()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetExpressCheckout(decimal amount, string currency_code, string item_name, string return_false_url, string webtype)
        {
            string hots = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/";

            NVPCodec encoder = new NVPCodec();
            encoder.Add("PAYMENTACTION", "Sale");
            //不允许客户改地址
            encoder.Add("ADDROVERRIDE", "1");
            encoder.Add("CURRENCYCODE", currency_code);
            encoder.Add("L_NAME0", item_name);
            //encoder.Add("L_NUMBER0", item_name);
            //encoder.Add("L_DESC0", item_name);
            encoder.Add("L_AMT0", amount.ToString());
            encoder.Add("L_QTY0", "1");
            double ft = double.Parse(amount.ToString());
            double amt = System.Math.Round(ft + 5.00f + 2.00f + 1.00f, 2);
            double maxamt = System.Math.Round(amt + 25.00f, 2);
            encoder.Add("AMT", ft.ToString());
            encoder.Add("RETURNURL", hots + "/Statistics/Return?orderid=" + item_name + "&price=" + amount + "&type=sadmincz");
            encoder.Add("CANCELURL", return_false_url);
            return Json(return_false_url);
            //NVPCodec decoder = PaypalController.SetExpressCheckout(encoder);
            //string ack = decoder["ACK"];
            //if (!string.IsNullOrEmpty(ack) && (ack.Equals("Success", System.StringComparison.OrdinalIgnoreCase) || ack.Equals("SuccessWithWarning", System.StringComparison.OrdinalIgnoreCase)))
            //{
            //    Session["TOKEN"] = decoder["token"];
            //    return Json(ConfigurationManager.AppSettings["RedirectURL"] + decoder["token"]);
            //}
            //else
            //{
            //    return Json(return_false_url);
            //    //LSGKE52H5FW236597  SGM7151LAAA

            //}
        }

    }
}