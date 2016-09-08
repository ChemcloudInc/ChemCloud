using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using ChemCloud.Web;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Controllers;
using ChemCloud.Web.Framework;
using com.paypal.sdk.util;
using ChemCloud.AliPay;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ChemCloud.DBUtility;
using System.Data;
using ChemColud.Shipping;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class OrderController : BaseMemberController
    {
        public OrderController()
        {

        }

        public JsonResult CalcFreight(long addressId, string datas)
        {
            if (string.IsNullOrEmpty(datas))
            {
                return Json(new { success = false, msg = "计算运费失败" });
            }
            List<object[]> objArrays = new List<object[]>();
            string[] strArrays = datas.Split(new char[] { '|' });
            for (int i = 0; i < strArrays.Length; i++)
            {
                List<CartItemModel> cartItemModels = JsonConvert.DeserializeObject<List<CartItemModel>>(strArrays[i]);
                List<string> strs = new List<string>();
                List<int> nums = new List<int>();
                decimal num = new decimal(0);
                foreach (CartItemModel cartItemModel in cartItemModels)
                {
                    string str = cartItemModel.skuId;
                    if (!strs.Contains(str))
                    {
                        strs.Add(str);
                        nums.Add(cartItemModel.count);
                    }
                    else
                    {
                        int num1 = strs.IndexOf(str);
                        int item = nums[num1];
                        nums[num1] = item + cartItemModel.count;
                    }
                    num = num + (cartItemModel.count * cartItemModel.price);
                }
                ShippingAddressInfo userShippingAddress = Instance<IShippingAddressService>.Create.GetUserShippingAddress(addressId);
                int cityId = 0;
                if (userShippingAddress != null)
                {
                    cityId = Instance<IRegionService>.Create.GetCityId(userShippingAddress.RegionIdPath);
                }
                decimal freight = Instance<IProductService>.Create.GetFreight(strs, nums, cityId);
                object[] objArray = new object[] { num, freight };
                objArrays.Add(objArray);
            }
            return Json(objArrays);
        }

        [HttpPost]
        public JsonResult IsPassAuth(long OrderId)
        {
            bool flag = ServiceHelper.Create<IApplyAmountService>().IsPassAuth(base.CurrentUser.Id, OrderId);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult IsOverAmount(decimal TotalAmount, int CoinType)
        {
            bool flag = ServiceHelper.Create<IOrganizationService>().AmountOver(base.CurrentUser.Id, TotalAmount, int.Parse(CoinType.ToString()));
            //bool flags = ServiceHelper.Create<IApplyAmountService>().IsPassAuth(base.CurrentUser.Id, OrderId);
            if (flag)
                return Json(new { success = true, msg = "success！" });
            else
                return Json(new { success = false, msg = "success！" });
        }
        public ActionResult ChargePay(string orderIds)
        {
            if (string.IsNullOrEmpty(orderIds))
            {
                return RedirectToAction("index", "userCenter", new { url = "/UserCapital", tar = "UserCapital" });
            }
            ViewBag.OrderIds = orderIds;
            ViewBag.CoinType = ConfigurationManager.AppSettings["CoinType"];
            Finance_Recharge frinfo = ServiceHelper.Create<IFinance_RechargeService>().GetFinance_RechargeInfo(long.Parse(orderIds));
            if (frinfo == null)
            {
                return RedirectToAction("index", "userCenter", new { url = "/UserCapital", tar = "UserCapital" });
            }
            else
            {
                ViewBag.RechargeMoney = frinfo.Recharge_Money;
                if (frinfo.Recharge_MoneyType == 1)
                {
                    ViewBag.RechargeMoneyType = "CNY";
                }
                else
                {
                    ViewBag.RechargeMoneyType = "USD";
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult DeleteInvoiceTitle(long id)
        {
            ServiceHelper.Create<IOrderService>().DeleteInvoiceTitle(id);
            return Json(true);
        }

        private string EncodePaymentId(string paymentId)
        {
            return paymentId.Replace(".", "-");
        }

        private void GetOrderProductsInfo(string skuIds, string counts, string collIds = null)
        {
            ShippingAddressInfo defaultUserShippingAddressByUserId = ServiceHelper.Create<IShippingAddressService>().GetDefaultUserShippingAddressByUserId(base.CurrentUser.Id);
            int cityId = 0;
            if (defaultUserShippingAddressByUserId != null)
            {
                cityId = Instance<IRegionService>.Create.GetCityId(defaultUserShippingAddressByUserId.RegionIdPath);
            }
            IEnumerable<long> nums = null;
            string[] strArrays = skuIds.Split(new char[] { ',' });
            string str = counts.TrimEnd(new char[] { ',' });
            char[] chrArray = new char[] { ',' };
            IEnumerable<int> nums1 =
                from t in str.Split(chrArray)
                select int.Parse(t);
            if (!string.IsNullOrEmpty(collIds))
            {
                string str1 = collIds.TrimEnd(new char[] { ',' });
                char[] chrArray1 = new char[] { ',' };
                nums =
                    from t in str1.Split(chrArray1)
                    select long.Parse(t);
            }
            IProductService productService = ServiceHelper.Create<IProductService>();
            int num2 = 0;
            int length = strArrays.Length;
            List<CartItemModel> list = strArrays.Select<string, CartItemModel>((string item) =>
            {
                SKUInfo sku = productService.GetSku(item);
                int num = nums1.ElementAt<int>(num2);
                long num1 = (nums != null ? nums.ElementAt<long>(num2) : 0);
                num2++;
                return new CartItemModel()
                {
                    skuId = item,
                    id = sku.ProductInfo.Id,
                    imgUrl = sku.ProductInfo.GetImage(ProductInfo.ImageSize.Size_50, 1),
                    name = sku.ProductInfo.ProductName,
                    shopId = sku.ProductInfo.ShopId,
                    price = GetSalePrice(sku.ProductInfo.Id, sku, new long?(num1), length),
                    count = num,
                    productCode = sku.ProductInfo.ProductCode
                };
            }).ToList();
            IShopService create = Instance<IShopService>.Create;
            IEnumerable<IGrouping<long, CartItemModel>> groupings =
                from a in list
                group a by a.shopId;
            List<ShopCartItemModel> shopCartItemModels = new List<ShopCartItemModel>();
            foreach (IGrouping<long, CartItemModel> nums2 in groupings)
            {
                IEnumerable<long> nums3 =
                    from r in nums2
                    select r.id;
                IEnumerable<int> nums4 =
                    from r in nums2
                    select r.count;
                ShopCartItemModel shopCartItemModel = new ShopCartItemModel()
                {
                    shopId = nums2.Key

                };
                shopCartItemModel.CartItemModels = (
                        from a in list
                        where a.shopId == shopCartItemModel.shopId
                        select a).ToList();
                shopCartItemModel.ShopName = create.GetShop(shopCartItemModel.shopId, false).ShopName;
                if (cityId != 0)
                {
                    shopCartItemModel.Freight = ServiceHelper.Create<IProductService>().GetFreight(nums3, nums4, cityId);
                }
                shopCartItemModel.UserCoupons = ServiceHelper.Create<ICouponService>().GetUserCoupon(shopCartItemModel.shopId, base.CurrentUser.Id, nums2.Sum<CartItemModel>((CartItemModel a) => a.price * a.count));
                shopCartItemModel.UserBonus = ServiceHelper.Create<IShopBonusService>().GetDetailToUse(shopCartItemModel.shopId, base.CurrentUser.Id, nums2.Sum<CartItemModel>((CartItemModel a) => a.price * a.count));
                shopCartItemModels.Add(shopCartItemModel);
            }
            ViewBag.CollIds = collIds;
            ViewBag.products = shopCartItemModels;
            base.ViewBag.totalAmount = list.Sum<CartItemModel>((CartItemModel item) => item.price * item.count);
            base.ViewBag.Freight = shopCartItemModels.Sum<ShopCartItemModel>((ShopCartItemModel a) => a.Freight);
            dynamic viewBag = base.ViewBag;
            dynamic obj = ViewBag.totalAmount;
            viewBag.orderAmount = obj + ViewBag.Freight;
        }

        private void GetOrderProductsInfo(string cartItemIds, long? regionId)
        {
            ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
            shippingAddressInfo = (!regionId.HasValue ? ServiceHelper.Create<IShippingAddressService>().GetDefaultUserShippingAddressByUserId(base.CurrentUser.Id) : ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddress(regionId.Value));
            int cityId = 0;
            if (shippingAddressInfo != null)
            {
                cityId = Instance<IRegionService>.Create.GetCityId(shippingAddressInfo.RegionIdPath);
            }
            CartHelper cartHelper = new CartHelper();
            IEnumerable<ShoppingCartItem> cartItems = null;
            if (!string.IsNullOrWhiteSpace(cartItemIds))
            {
                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from t in cartItemIds.Split(chrArray)
                    select long.Parse(t);
                cartItems = ServiceHelper.Create<ICartService>().GetCartItems(nums);
            }
            else
            {
                cartItems = cartHelper.GetCart(base.CurrentUser.Id).Items;
            }
            IProductService productService = ServiceHelper.Create<IProductService>();
            IShopService shopService = ServiceHelper.Create<IShopService>();
            List<CartItemModel> list = cartItems.Select<ShoppingCartItem, CartItemModel>((ShoppingCartItem item) =>
            {
                ProductInfo product = productService.GetProduct(item.ProductId);
                SKUInfo sku = productService.GetSku(item.SkuId);
                return new CartItemModel()
                {
                    skuId = item.SkuId,
                    id = product.Id,
                    imgUrl = product.GetImage(ProductInfo.ImageSize.Size_50, 1),
                    name = product.ProductName,
                    price = sku.SalePrice,
                    shopId = product.ShopId,
                    count = item.Quantity,
                    productCode = product.ProductCode
                };
            }).ToList();
            IEnumerable<IGrouping<long, CartItemModel>> groupings =
                from a in list
                group a by a.shopId;
            List<ShopCartItemModel> shopCartItemModels = new List<ShopCartItemModel>();
            foreach (IGrouping<long, CartItemModel> nums1 in groupings)
            {
                IEnumerable<long> nums2 =
                    from r in nums1
                    select r.id;
                IEnumerable<int> nums3 =
                    from r in nums1
                    select r.count;
                ShopCartItemModel shopCartItemModel = new ShopCartItemModel()
                {
                    shopId = nums1.Key,

                    Freight = (cityId <= 0 ? new decimal(0) : ServiceHelper.Create<IProductService>().GetFreight(nums2, nums3, cityId)),
                };

                shopCartItemModel.CartItemModels = (
                        from a in list
                        where a.shopId == shopCartItemModel.shopId
                        select a).ToList();
                shopCartItemModel.ShopName = shopService.GetShop(shopCartItemModel.shopId, false).ShopName;
                shopCartItemModel.UserCoupons = ServiceHelper.Create<ICouponService>().GetUserCoupon(shopCartItemModel.shopId, base.CurrentUser.Id, nums1.Sum<CartItemModel>((CartItemModel a) => a.price * a.count));
                shopCartItemModel.UserBonus = ServiceHelper.Create<IShopBonusService>().GetDetailToUse(shopCartItemModel.shopId, base.CurrentUser.Id, nums1.Sum<CartItemModel>((CartItemModel a) => a.price * a.count));

                shopCartItemModels.Add(shopCartItemModel);
            }
            ViewBag.products = shopCartItemModels;
            base.ViewBag.totalAmount = list.Sum<CartItemModel>((CartItemModel item) => item.price * item.count);
            base.ViewBag.Freight = shopCartItemModels.Sum<ShopCartItemModel>((ShopCartItemModel a) => a.Freight);
            dynamic viewBag = base.ViewBag;
            dynamic obj = ViewBag.totalAmount;
            viewBag.orderAmount = obj + ViewBag.Freight;
        }

        public JsonResult GetPayPwd()
        {
            if (string.IsNullOrWhiteSpace(base.CurrentUser.PayPwd))
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        private string GetProductNameDescriptionFromOrders(IEnumerable<OrderInfo> orders)
        {
            string str;
            List<string> strs = new List<string>();
            foreach (OrderInfo list in orders.ToList())
            {
                strs.AddRange(
                    from t in list.OrderItemInfo
                    select t.ProductName);
            }
            if (strs.Count() > 1)
            {
                object[] objArray = new object[] { strs.ElementAt<string>(0), " 等", strs.Count(), "种产品" };
                str = string.Concat(objArray);
            }
            else
            {
                str = strs.ElementAt<string>(0);
            }
            return str;
        }

        private decimal GetSalePrice(long productId, SKUInfo sku, long? collid, int Count)
        {
            decimal salePrice = sku.SalePrice;
            if (collid.HasValue && collid.Value != 0 && Count > 1)
            {
                CollocationSkuInfo colloSku = ServiceHelper.Create<ICollocationService>().GetColloSku(collid.Value, sku.Id);
                if (colloSku != null)
                {
                    salePrice = colloSku.Price;
                }
            }
            else if (Count == 1)
            {
                LimitTimeMarketInfo limitTimeMarketItemByProductId = ServiceHelper.Create<ILimitTimeBuyService>().GetLimitTimeMarketItemByProductId(productId);
                if (limitTimeMarketItemByProductId != null)
                {
                    salePrice = limitTimeMarketItemByProductId.Price;
                }
            }
            return salePrice;
        }

        private void GetShippingAddress(long? regionId)
        {
            if (regionId.HasValue)
            {
                ViewBag.address = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddress(regionId.Value);
                return;
            }
            ViewBag.address = ServiceHelper.Create<IShippingAddressService>().GetDefaultUserShippingAddressByUserId(base.CurrentUser.Id);
        }

        [HttpPost]
        public JsonResult GetUserShippingAddresses()
        {
            ShippingAddressInfo[] array = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id).ToArray();
            var variable =
                from item in array
                select new { id = item.Id, fullRegionName = item.RegionFullName, address = item.Address, phone = item.Phone, shipTo = item.ShipTo, fullRegionIdPath = item.RegionIdPath };
            return Json(variable);
        }

        /// <summary>
        /// 支付页面
        /// </summary>
        /// <param name="orderIds">订单号</param>
        /// <param name="type">0全额付款1分期付款</param>
        /// <param name="paytype">支付类型(3供应商认证支付,2产品认证支付，1排名付费支付,0订单支付 4定制合成 5代理采购)</param>
        /// <returns></returns>
        public ActionResult Pay(string orderIds, string type = "0", string paytype = "0", string targetid = "", string orderprice = "0")
        {
            /*当前用户类型*/
            //ViewBag.UserType = base.CurrentUser.UserType;

            if (paytype == "0")
            {
                #region 订单支付
                string str;
                if (string.IsNullOrEmpty(orderIds))
                {
                    return RedirectToAction("index", "userCenter", new { url = "/userOrder", tar = "userOrder" });
                }
                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from item in orderIds.Split(chrArray)
                    select long.Parse(item);
                if (ServiceHelper.Create<IOrderService>().GetOrders(nums).Any((OrderInfo item) =>
                {
                    if (type == "1")
                    {
                        if (item.OrderStatus != OrderInfo.OrderOperateStatus.FQPAY && item.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (item.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {
                            return true;
                        }
                    }
                    return item.UserId != base.CurrentUser.Id;
                }))
                {
                    return RedirectToAction("index", "userCenter", new { url = string.Concat("/userOrder?orderids=", orderIds), tar = "userOrder" });
                }
                SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
                ViewBag.Logo = siteSettings.Logo;
                IOrderService orderService = ServiceHelper.Create<IOrderService>();
                List<OrderInfo> list = orderService.GetOrders(nums).Where((OrderInfo item) =>
                {
                    if (type == "1")
                    {
                        if (item.OrderStatus != OrderInfo.OrderOperateStatus.FQPAY && item.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (item.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {
                            return false;
                        }
                    }
                    return item.UserId == base.CurrentUser.Id;
                }).ToList();
                bool flag = false;
                List<OrderInfo> orderInfos = new List<OrderInfo>();
                foreach (OrderInfo orderInfo in list)
                {
                    orderInfo.HaveDelProduct = orderService.IsHaveNoOnSaleProduct(orderInfo.Id);
                    if (!orderInfo.HaveDelProduct)
                    {
                        continue;
                    }
                    orderInfos.Add(orderInfo);
                    flag = true;
                    orderService.PlatformCloseOrder(orderInfo.Id, "系统自动", "有未销售的产品，请联系商家或平台");
                }
                if (flag)
                {
                    foreach (OrderInfo orderInfo1 in orderInfos)
                    {
                        list.Remove(orderInfo1);
                    }
                    throw new HimallException("有订单产品处于非销售状态，请手动处理。");
                }
                ViewBag.HaveNoSalePro = flag;
                if (list == null || list.Count == 0)
                {
                    return RedirectToAction("index", "userCenter", new { url = "/userOrder", tar = "userOrder" });
                }
                ViewBag.Orders = list;
                decimal num = list.Sum<OrderInfo>((OrderInfo a) => a.OrderTotalAmount);
                if (num == new decimal(0))
                {
                    ViewBag.TotalAmount = num;
                    return View("PayConfirm");
                }
                string productNameDescriptionFromOrders = GetProductNameDescriptionFromOrders(list);
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
                string str2 = string.Concat(str1, "/Pay/Return/{0}");
                string str3 = string.Concat(str1, "/Pay/Notify/{0}");
                IEnumerable<Plugin<IPaymentPlugin>> plugins =
                    from item in PluginsManagement.GetPlugins<IPaymentPlugin>(true)
                    where item.Biz.SupportPlatforms.Contains<PlatformType>(PlatformType.PC)
                    select item;
                char[] chrArray1 = new char[] { ',' };
                IEnumerable<OrderPayInfo> orderPayInfos =
                    from item in orderIds.Split(chrArray1)
                    select new OrderPayInfo()
                    {
                        PayId = 0,
                        OrderId = long.Parse(item)
                    };
                long num1 = orderService.SaveOrderPayInfo(orderPayInfos, PlatformType.PC);
                string str4 = num1.ToString();
                IEnumerable<PaymentModel> paymentModels = plugins.Select<Plugin<IPaymentPlugin>, PaymentModel>((Plugin<IPaymentPlugin> item) =>
                {
                    string empty = string.Empty;
                    try
                    {
                        empty = item.Biz.GetRequestUrl(string.Format(str2, EncodePaymentId(item.PluginInfo.PluginId)), string.Format(str3, EncodePaymentId(item.PluginInfo.PluginId)), str4, num, productNameDescriptionFromOrders, null);
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
                ViewBag.TotalAmount = num;

                ViewBag.order = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderIds));


                //支付类型(3供应商认证支付,2产品认证支付，1排名付费支付,0订单支付，4定制合成，5代理采购)
                ViewBag.paytype = paytype;
                //支付对象的id，用来更新最终支付状态
                ViewBag.targetid = targetid;
                ViewBag.fqfirst = "";
                //分期操作
                if (!string.IsNullOrEmpty(type) && type == "1")
                {
                    FQPayment fqp = new FQPayment();
                    fqp = ServiceHelper.Create<IFQPaymentService>().GetFQPaymentInfo(long.Parse(orderIds), base.CurrentUser.Id);
                    if (fqp == null)
                    {
                        FQPayment fqpinfo = new FQPayment()
                        {
                            orderId = long.Parse(orderIds),
                            TolPrice = num,
                            RealPrice = num * int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayMinPrice")) / 100,
                            PayTime = DateTime.Now,
                            LeftPrice = num - (num * int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayMinPrice")) / 100),
                            MemberId = base.CurrentUser.Id,
                            Status = 0
                        };
                        if (ServiceHelper.Create<IFQPaymentService>().AddFQPayment(fqpinfo))
                        {
                            ViewBag.fqfirst = 1;
                            Log.Info("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "时成功创建分期订单，订单号：" + orderIds);
                        }
                        else
                        {
                            Log.Info("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "时未能成功创建分期订单，订单号：" + orderIds);
                        }
                        ViewBag.PayStatus = 0;
                        fqp = fqpinfo;
                    }
                    else
                    {
                        ViewBag.PayStatus = fqp.Status;
                    }
                    if (ViewBag.PayStatus != 2)
                    {
                        ViewBag.percentpay = int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayMinPrice"));
                        ViewBag.LeftPrice = fqp.LeftPrice;
                        ViewBag.RealPrice = fqp.RealPrice;
                    }
                }
                else
                {
                    ViewBag.LeftPrice = 0;
                    ViewBag.RealPrice = 0;
                }

                ViewBag.Step = 1;
                string cointype = ConfigurationManager.AppSettings["CoinType"];
                ViewBag.CoinType = cointype;
                if (cointype == "1")
                {
                    ViewBag.paydanwei = "CNY";
                }
                else if (cointype == "2")
                {
                    ViewBag.paydanwei = "USD";
                }
                ViewBag.UnpaidTimeout = siteSettings.UnpaidTimeout;
                //CapitalInfo capitalInfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfo(base.CurrentUser.Id);
                Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                if (fw != null)
                {
                    ViewBag.Capital = fw.Wallet_UserLeftMoney;
                }
                else
                {
                    ViewBag.Capital = 0;
                }
                ViewBag.PayId = num1;
                return View(paymentModels);
                #endregion
            }
            else
            {
                #region 其他支付


                if (paytype == "7")//我要采购
                {
                    decimal amount = 0;
                    string strsql = string.Format("select TotalPrice from dbo.ChemCloud_IWantToSupply where Id='" + orderIds + "';");
                    DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["TotalPrice"] != null)
                        {
                            amount = decimal.Parse(dt.Rows[0]["TotalPrice"].ToString());
                        }
                    }
                    ViewBag.SortCost = amount;
                }

                if (paytype == "6")//分析鉴定
                {
                    decimal amount = 0;
                    string strsql = string.Format("SELECT ServiceCharge FROM dbo.ChemCloud_Product_Analysis where Id='" + orderIds + "';");
                    DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["ServiceCharge"] != null)
                        {
                            amount = decimal.Parse(dt.Rows[0]["ServiceCharge"].ToString());
                        }
                    }
                    ViewBag.SortCost = amount;
                }

                if (paytype == "5")//代理采购
                {
                    decimal amount = 0;
                    string strsql = string.Format("SELECT * FROM dbo.ChemCloud_OrderPurchasing where OrderNum='" + orderIds + "';");
                    DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["ProductPrice"] != null && dt.Rows[0]["ProductCount"] != null)
                        {
                            amount = decimal.Parse(dt.Rows[0]["ProductPrice"].ToString()) * decimal.Parse(dt.Rows[0]["ProductCount"].ToString());
                        }
                    }
                    ViewBag.SortCost = amount;
                }
                else if (paytype == "4") //定制合成
                {
                    decimal amount = 0;
                    string strsql = string.Format("SELECT * FROM dbo.ChemCloud_OrderSynthesis where OrderNumber='" + orderIds + "';");
                    DataTable dt = DbHelperSQL.QueryDataTable(strsql);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Price"] != null && dt.Rows[0]["ProductCount"] != null)
                        {
                            amount = decimal.Parse(dt.Rows[0]["Price"].ToString()) * decimal.Parse(dt.Rows[0]["ProductCount"].ToString());
                        }
                    }
                    ViewBag.SortCost = amount;
                }
                else if (paytype == "3")//供应商认证支付
                {
                    ViewBag.SortCost = decimal.Parse(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("SupplierCertificationAmount") == null ? "0" : ServiceHelper.Create<ISiteSettingService>().GetSiteValue("SupplierCertificationAmount"));
                }
                else if (paytype == "2")//产品认证支付
                {
                    ViewBag.SortCost = decimal.Parse(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("ProductCertificationAmount") == null ? "0" : ServiceHelper.Create<ISiteSettingService>().GetSiteValue("ProductCertificationAmount"));
                }
                else if (paytype == "1")//排名付费支付
                {
                    ViewBag.SortCost = decimal.Parse(ServiceHelper.Create<ISiteSettingService>().GetSiteValue("SortCost") == null ? "0" : ServiceHelper.Create<ISiteSettingService>().GetSiteValue("SortCost"));
                }

                //支付类型(3供应商认证支付,2产品认证支付，1排名付费支付,0订单支付，4定制合成，5代理采购)
                ViewBag.paytype = paytype;
                //支付对象的id，用来更新最终支付状态
                ViewBag.targetid = targetid;

                string str;

                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from item in orderIds.Split(chrArray)
                    select long.Parse(item);

                SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
                ViewBag.Logo = siteSettings.Logo;
                IOrderService orderService = ServiceHelper.Create<IOrderService>();
                List<OrderInfo> list = orderService.GetOrders(nums).Where((OrderInfo item) =>
                {
                    if (type == "1")
                    {
                        if (item.OrderStatus != OrderInfo.OrderOperateStatus.FQPAY && item.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (item.OrderStatus != OrderInfo.OrderOperateStatus.WaitPay)
                        {
                            return false;
                        }
                    }
                    return item.UserId == base.CurrentUser.Id;
                }).ToList();
                bool flag = false;
                List<OrderInfo> orderInfos = new List<OrderInfo>();
                ViewBag.HaveNoSalePro = flag;

                ViewBag.Orders = list;
                decimal num = list.Sum<OrderInfo>((OrderInfo a) => a.OrderTotalAmount);

                //string productNameDescriptionFromOrders = GetProductNameDescriptionFromOrders(list);
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
                string str2 = string.Concat(str1, "/Pay/Return/{0}");
                string str3 = string.Concat(str1, "/Pay/Notify/{0}");
                IEnumerable<Plugin<IPaymentPlugin>> plugins =
                    from item in PluginsManagement.GetPlugins<IPaymentPlugin>(true)
                    where item.Biz.SupportPlatforms.Contains<PlatformType>(PlatformType.PC)
                    select item;
                char[] chrArray1 = new char[] { ',' };
                IEnumerable<OrderPayInfo> orderPayInfos =
                    from item in orderIds.Split(chrArray1)
                    select new OrderPayInfo()
                    {
                        PayId = 0,
                        OrderId = long.Parse(item)
                    };
                long num1 = orderService.SaveOrderPayInfo(orderPayInfos, PlatformType.PC);
                string str4 = num1.ToString();
                IEnumerable<PaymentModel> paymentModels = plugins.Select<Plugin<IPaymentPlugin>, PaymentModel>((Plugin<IPaymentPlugin> item) =>
                {
                    string empty = string.Empty;
                    try
                    {
                        //empty = item.Biz.GetRequestUrl(string.Format(str2, EncodePaymentId(item.PluginInfo.PluginId)), string.Format(str3, EncodePaymentId(item.PluginInfo.PluginId)), str4, num, productNameDescriptionFromOrders, null);
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
                ViewBag.TotalAmount = num;

                //支付类型(3供应商认证支付,2产品认证支付，1排名付费支付,0订单支付)
                ViewBag.paytype = paytype;

                //分期操作
                if (!string.IsNullOrEmpty(type) && type == "1")
                {
                    FQPayment fqp = new FQPayment();
                    fqp = ServiceHelper.Create<IFQPaymentService>().GetFQPaymentInfo(long.Parse(orderIds), base.CurrentUser.Id);
                    if (fqp == null)
                    {
                        FQPayment fqpinfo = new FQPayment()
                        {
                            orderId = long.Parse(orderIds),
                            TolPrice = num,
                            RealPrice = num * int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayMinPrice")) / 100,
                            PayTime = DateTime.Now,
                            LeftPrice = num - (num * int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayMinPrice")) / 100),
                            MemberId = base.CurrentUser.Id,
                            Status = 0
                        };
                        if (ServiceHelper.Create<IFQPaymentService>().AddFQPayment(fqpinfo))
                        {
                            Log.Info("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "时成功创建分期订单，订单号：" + orderIds);
                        }
                        else
                        {
                            Log.Info("用户：" + base.CurrentUser.Id + "在" + DateTime.Now + "时未能成功创建分期订单，订单号：" + orderIds);
                        }
                        ViewBag.PayStatus = 0;
                        fqp = fqpinfo;
                    }
                    else
                    {
                        ViewBag.PayStatus = fqp.Status;
                    }
                    if (ViewBag.PayStatus != 2)
                    {
                        ViewBag.percentpay = int.Parse(ChemCloud.Web.Framework.ServiceHelper.Create<ChemCloud.IServices.IChemCloud_DictionariesService>().GetValueBYKey("PayMinPrice"));
                        ViewBag.LeftPrice = fqp.LeftPrice;
                        ViewBag.RealPrice = fqp.RealPrice;
                    }
                }
                else
                {
                    ViewBag.LeftPrice = 0;
                    ViewBag.RealPrice = 0;
                }

                ViewBag.Step = 1;
                string cointype = ConfigurationManager.AppSettings["CoinType"];
                ViewBag.CoinType = cointype;
                if (cointype == "1")
                {
                    ViewBag.paydanwei = "CNY";
                }
                else if (cointype == "2")
                {
                    ViewBag.paydanwei = "USD";
                }
                ViewBag.UnpaidTimeout = siteSettings.UnpaidTimeout;
                //CapitalInfo capitalInfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfo(base.CurrentUser.Id);
                Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                if (fw != null)
                {
                    ViewBag.Capital = fw.Wallet_UserLeftMoney;
                }
                else
                {
                    ViewBag.Capital = 0;
                }
                ViewBag.UserType = base.CurrentUser.UserType;
                ViewBag.PayId = num1;
                return View(paymentModels);
                #endregion
            }


        }


        [HttpPost]
        public ActionResult PayConfirm(string orderIds)
        {
            if (string.IsNullOrEmpty(orderIds))
            {
                return RedirectToAction("index", "userCenter", new { url = "/userOrder", tar = "userOrder" });
            }
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in orderIds.Split(chrArray)
                select long.Parse(item);
            ServiceHelper.Create<IOrderService>().ConfirmZeroOrder(nums, base.CurrentUser.Id);
            return RedirectToAction("ReturnSuccess", "pay", new { id = orderIds });
        }

        [HttpPost]
        public JsonResult SaveInvoiceTitle(string name)
        {
            InvoiceTitleInfo invoiceTitleInfo = new InvoiceTitleInfo()
            {
                Name = name,
                UserId = base.CurrentUser.Id,
                IsDefault = 0
            };
            long num = ServiceHelper.Create<IOrderService>().SaveInvoiceTitle(invoiceTitleInfo);
            return Json(num);
        }

        public ActionResult Submit(long cartItemId)
        {
            //long num;
            //num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            //ShoppingCartInfo cart = (new CartHelper()).GetCart(num, cartItemId);

            //IProductService productService = ServiceHelper.Create<IProductService>();
            //IShopService shopService = ServiceHelper.Create<IShopService>();

            //List<ShoppingCartItem> cartlist = cart.Items.ToList();
            //ProductInfo product = new ProductInfo();
            //ShopInfo shop = new ShopInfo();
            //foreach (var item in cartlist)
            //{
            //    product = productService.GetProduct(item.ProductId);
            //    shop = shopService.GetShop(product.ShopId, false);
            //    item.ProductId = product.Id;
            //    item.imgUrl = string.Concat(product.ImagePath, "/1_50.png");
            //    item.name = product.ProductName;
            //    item.shopName = shop.ShopName;
            //    item.productcode = product.ProductCode;
            //}
            //ViewBag.ProConType = cartlist.FirstOrDefault() != null ? cartlist.FirstOrDefault().CoinType : "CNY";
            //ViewBag.shopId = cartlist.FirstOrDefault() == null ? 0 : cartlist.FirstOrDefault().ShopId;
            //ViewBag.shopName = cartlist.FirstOrDefault() == null ? "" : cartlist.FirstOrDefault().shopName;

            //ViewBag.cartlist = cartlist; //购物车信息


            //ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            //long orderid = _orderBO.GenerateOrderNumber();
            //ViewBag.billno = orderid; //生成订单号


            //string InsurancefeeMaxValue = "1";
            //SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            //if (siteSettings != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(siteSettings.InsurancefeeMaxValue))
            //    {
            //        InsurancefeeMaxValue = siteSettings.InsurancefeeMaxValue;
            //    }
            //}
            //ViewBag.InsurancefeeMaxValue = InsurancefeeMaxValue;

            long num;
            num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
            ShoppingCartInfo cart = ServiceHelper.Create<ICartService>().GetCart(num);
            IProductService productService = ServiceHelper.Create<IProductService>();
            IShopService shopService = ServiceHelper.Create<IShopService>();
            List<ShoppingCartItem> cartlist = cart.Items.ToList();
            ProductInfo product = new ProductInfo();
            ShopInfo shop = new ShopInfo();
            foreach (var item in cartlist)
            {
                product = productService.GetProduct(item.ProductId);
                shop = shopService.GetShop(product.ShopId, false);
                item.ProductId = product.Id;
                item.imgUrl = product.ImagePath;
                item.name = product.ProductName;
                item.shopName = shop.ShopName;
                item.productcode = product.ProductCode;
                item.shopId = product.ShopId;
            }
            ViewBag.cartshoplist = cartlist.DistinctBy(p => p.ShopId).ToList();

            ViewBag.cartproductlist = cartlist;

            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            long orderid = _orderBO.GenerateOrderNumber();
            ViewBag.billno = orderid; //生成订单号


            string InsurancefeeMaxValue = "1";
            SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            if (siteSettings != null)
            {
                if (!string.IsNullOrWhiteSpace(siteSettings.InsurancefeeMaxValue))
                {
                    InsurancefeeMaxValue = siteSettings.InsurancefeeMaxValue;
                }
            }
            ViewBag.InsurancefeeMaxValue = InsurancefeeMaxValue;


            /*判断是否已经添加了购物地址*/
            string iseditshippingaddress = "0";
            List<ShippingAddressInfo> _ShippingAddressInfo = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id).ToList();
            if (_ShippingAddressInfo.Count > 0)
            {
                iseditshippingaddress = "1";
            }
            ViewBag.iseditshippingaddress = iseditshippingaddress;

            IQueryable<ShippingAddressInfo> shippingAddress = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id);

            //物流费用相关
            if (shippingAddress != null && shippingAddress.Count() > 0)
            {
                List<ShippingAddressInfo> tmpAddress = shippingAddress.Where(x => x.IsDefault == true).ToList();
                GetShipment(ViewBag.cartshoplist, cartlist, tmpAddress[0]);
            }


            return View(shippingAddress);

        }


        [HttpPost]
        public string GetShipmentDetail(ShopCartCost shopCart)
        {
            string result = string.Empty;
            try
            {
                // ShopCartCost shopCart = Newtonsoft.Json.JsonConvert.DeserializeObject<ShopCartCost>(ship);
                if (shopCart != null)
                {
                    long num;
                    num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
                    ShoppingCartInfo cart = ServiceHelper.Create<ICartService>().GetCart(num);
                    IProductService productService = ServiceHelper.Create<IProductService>();
                    IShopService shopService = ServiceHelper.Create<IShopService>();
                    List<ShoppingCartItem> cartlist = cart.Items.ToList();
                    ProductInfo product = new ProductInfo();
                    ShopInfo shop = new ShopInfo();
                    foreach (var item in cartlist)
                    {
                        product = productService.GetProduct(item.ProductId);
                        shop = shopService.GetShop(product.ShopId, false);
                        item.ProductId = product.Id;
                        item.imgUrl = product.ImagePath;
                        item.name = product.ProductName;
                        item.shopName = shop.ShopName;
                        item.productcode = product.ProductCode;
                        item.shopId = product.ShopId;
                    }
                    List<ShoppingCartItem> cartshoplist = cartlist.DistinctBy(p => p.ShopId).Where(x => x.ShopId == shopCart.ShopId).ToList();

                    List<ShoppingCartItem> cartproductlist = cartlist.Where(x => x.ShopId == shopCart.ShopId).ToList();

                    List<ShippingAddressInfo> _ShippingAddressInfo = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id).ToList();

                    ShippingAddressInfo addressInfo = null;
                    if (_ShippingAddressInfo.Count > 0)
                    {
                        addressInfo = _ShippingAddressInfo.Where(x => x.Id == shopCart.DestId).SingleOrDefault();
                    }
                    ShoppingCartItem cartShop = null;
                    if (cartshoplist.Count > 0)
                    {
                        cartShop = cartshoplist[0];
                    }
                    if (cartShop != null)
                    {

                        GetShipment(cartShop, cartproductlist, addressInfo, shopCart);
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(cartShop);
                    }

                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private void GetShipment(ShoppingCartItem cartshoplist, List<ShoppingCartItem> cartList, ShippingAddressInfo shippingAddress, ShopCartCost cartCost)
        {
            IProductService productService = ServiceHelper.Create<IProductService>();
            IShopService shopService = ServiceHelper.Create<IShopService>();
            ShopInfo shopInfo;
            ProductInfo product;

            product = productService.GetProduct(cartshoplist.ProductId);
            if (product != null)
            {
                shopInfo = shopService.GetShop(product.ShopId, false);

                ShipmentEx shipment = null;

                FedExQueryAdv ship = new FedExQueryAdv();

                ship.ShopId = shopInfo.Id;
                ship.OrigId = shopInfo.CompanyRegionId;
                ship.OrigAddress = shopInfo.CompanyAddress;
                ship.OrigPostCode = shopInfo.ZipCode;

                ship.DestId = shippingAddress.Id;


                ship.CoinType = ConfigurationManager.AppSettings["CoinType"] == "1" ? "CNY" : "USD";
                var appSettings = ConfigurationManager.AppSettings;
                ship.FedexKey = appSettings["FedExKey"];
                ship.FedexPassword = appSettings["FedExPassword"];
                ship.FedexAccountNumber = appSettings["FedExAccountNumber"];
                ship.FedexMeterNumber = appSettings["FedExMeterNumber"];

                ship.PackagesList = new List<ShipPackage>();
                List<ShoppingCartItem> tmpList = cartList.Where(x => x.shopId == cartshoplist.ShopId).ToList();
                ShipPackage package = null;
                foreach (ShoppingCartItem cart in tmpList)
                {
                    package = new ShipPackage() { Num = cart.Quantity, PackingUnit = cart.PackingUnit };
                    ship.PackagesList.Add(package);
                }

                switch (cartCost.FreightType)
                {
                    case "FedEx":
                        shipment = ServiceHelper.Create<IShipmentService>().CalcPriceAdv(ship);
                        break;
                    case "SF":
                        shipment = ServiceHelper.Create<IShipmentService>().CalcSFCostAdv(ship);
                        break;
                }



                if (shipment != null)
                {
                    cartshoplist.ShipmentValue = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);
                    //item.ShipmentValue
                    if (shipment.RateValue != null)
                    {
                        cartshoplist.ShipmentCost = shipment.RateValue.TotalCharges;
                    }
                }
            }



        }

        private void GetShipment(List<ShoppingCartItem> cartshoplist, List<ShoppingCartItem> cartList, ShippingAddressInfo shippingAddress)
        {
            IProductService productService = ServiceHelper.Create<IProductService>();
            IShopService shopService = ServiceHelper.Create<IShopService>();
            ShopInfo shopInfo;
            ProductInfo product;
            foreach (ShoppingCartItem item in cartshoplist)
            {
                product = productService.GetProduct(item.ProductId);
                if (product != null)
                {
                    shopInfo = shopService.GetShop(product.ShopId, false);

                    ShipmentEx shipment = null;

                    FedExQueryAdv ship = new FedExQueryAdv();

                    ship.ShopId = shopInfo.Id;
                    ship.OrigId = shopInfo.CompanyRegionId;
                    ship.OrigAddress = shopInfo.CompanyAddress;
                    ship.OrigPostCode = shopInfo.ZipCode;

                    ship.DestId = shippingAddress.Id;


                    ship.CoinType = ConfigurationManager.AppSettings["CoinType"] == "1" ? "CNY" : "USD";
                    var appSettings = ConfigurationManager.AppSettings;
                    ship.FedexKey = appSettings["FedExKey"];
                    ship.FedexPassword = appSettings["FedExPassword"];
                    ship.FedexAccountNumber = appSettings["FedExAccountNumber"];
                    ship.FedexMeterNumber = appSettings["FedExMeterNumber"];

                    ship.PackagesList = new List<ShipPackage>();
                    List<ShoppingCartItem> tmpList = cartList.Where(x => x.shopId == item.ShopId).ToList();
                    ShipPackage package = null;
                    foreach (ShoppingCartItem cart in tmpList)
                    {
                        package = new ShipPackage() { Num = cart.Quantity, PackingUnit = cart.PackingUnit };
                        ship.PackagesList.Add(package);
                    }

                    shipment = ServiceHelper.Create<IShipmentService>().CalcPriceAdv(ship);


                    if (shipment != null)
                    {
                        item.ShipmentValue = Newtonsoft.Json.JsonConvert.SerializeObject(shipment);
                        //item.ShipmentValue
                        if (shipment.RateValue != null)
                        {
                            item.ShipmentCost = shipment.RateValue.TotalCharges;
                        }
                    }
                }

            }

        }

        public ActionResult SubmitByProductId(string skuIds, string counts, long? regionId, string collpids = null)
        {
            int num;
            int num1;
            int num2;
            dynamic obj;
            IOrderService orderService = ServiceHelper.Create<IOrderService>();
            ViewBag.Logo = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings().Logo;
            ViewBag.Member = base.CurrentUser;
            GetOrderProductsInfo(skuIds, counts, collpids);
            GetShippingAddress(regionId);
            IMemberIntegralService memberIntegralService = ServiceHelper.Create<IMemberIntegralService>();
            MemberIntegralExchangeRules integralChangeRule = memberIntegralService.GetIntegralChangeRule();
            MemberIntegral memberIntegral = memberIntegralService.GetMemberIntegral(base.CurrentUser.Id);
            dynamic viewBag = base.ViewBag;
            num = (integralChangeRule == null ? 0 : integralChangeRule.IntegralPerMoney);
            viewBag.IntegralPerMoney = num;
            dynamic viewBag1 = base.ViewBag;
            num1 = (integralChangeRule == null ? 0 : integralChangeRule.MoneyPerIntegral);
            viewBag1.MoneyPerIntegral = num1;
            dynamic obj1 = base.ViewBag;
            num2 = (memberIntegral == null ? 0 : memberIntegral.AvailableIntegrals);
            obj1.Integral = num2;
            dynamic viewBag2 = base.ViewBag;
            obj = (ViewBag.MoneyPerIntegral == 0 ? 0 : Math.Floor(ViewBag.totalAmount / ViewBag.MoneyPerIntegral));
            viewBag2.TotalIntegral = obj;
            ViewBag.collIds = collpids;
            ViewBag.skuIds = skuIds;
            ViewBag.counts = counts;
            ViewBag.InvoiceTitle = orderService.GetInvoiceTitles(base.CurrentUser.Id);
            ViewBag.InvoiceContext = orderService.GetInvoiceContexts();
            return View("Submit");
        }

        [HttpPost]
        public JsonResult SubmitOrder(string cartItemIds, long recieveAddressId, string couponIds, int invoiceType, string invoiceTitle, string invoiceContext, int integral = 0)
        {
            IEnumerable<long> items;
            IEnumerable<long> nums;
            IEnumerable<string[]> strArrays;
            IOrderService orderService = ServiceHelper.Create<IOrderService>();
            if (!string.IsNullOrWhiteSpace(cartItemIds))
            {
                char[] chrArray = new char[] { ',' };
                items =
                    from item in cartItemIds.Split(chrArray)
                    select long.Parse(item);
            }
            else
            {
                ShoppingCartInfo cart = ServiceHelper.Create<ICartService>().GetCart(base.CurrentUser.Id);
                items =
                    from item in cart.Items
                    select item.Id;
            }
            IEnumerable<string> strs = null;
            if (!string.IsNullOrEmpty(couponIds))
            {
                strs = couponIds.Split(new char[] { ',' });
            }
            if (integral < 0)
            {
                throw new HimallException("兑换积分数量不正确");
            }
            OrderCreateModel orderCreateModel = new OrderCreateModel()
            {
                CartItemIds = items
            };
            OrderCreateModel orderCreateModel1 = orderCreateModel;
            if (strs == null)
            {
                nums = null;
            }
            else
            {
                nums =
                    from p in strs
                    select long.Parse(p.Split(new char[] { '\u005F' })[0]);
            }
            orderCreateModel1.CouponIds = nums;
            OrderCreateModel orderCreateModel2 = orderCreateModel;
            if (strs == null)
            {
                strArrays = null;
            }
            else
            {
                strArrays =
                    from p in strs
                    select p.Split(new char[] { '\u005F' });
            }
            orderCreateModel2.CouponIdsStr = strArrays;
            orderCreateModel.CurrentUser = base.CurrentUser;
            orderCreateModel.Integral = integral;
            orderCreateModel.Invoice = (InvoiceType)invoiceType;
            orderCreateModel.InvoiceTitle = invoiceTitle;
            orderCreateModel.InvoiceContext = invoiceContext;
            orderCreateModel.ReceiveAddressId = recieveAddressId;
            List<OrderInfo> orderInfos = orderService.CreateOrder(orderCreateModel);
            bool flag = false;
            if (orderInfos.Sum<OrderInfo>((OrderInfo a) => a.OrderTotalAmount) == new decimal(0))
            {
                flag = true;
            }
            IEnumerable<long> array = (
                from item in orderInfos
                select item.Id).ToArray();
            return Json(new { success = true, orderIds = array, redirect = flag });
        }

        [HttpPost]
        public JsonResult SubmitOrderByProductId(string skuIds, string counts, long recieveAddressId, string couponIds, int invoiceType, string invoiceTitle, string invoiceContext, int integral = 0, string collIds = "")
        {
            IEnumerable<long> nums;
            IEnumerable<string[]> strArrays;
            string[] strArrays1 = skuIds.Split(new char[] { ',' });
            string str = counts.TrimEnd(new char[] { ',' });
            char[] chrArray = new char[] { ',' };
            IEnumerable<int> nums1 =
                from t in str.Split(chrArray)
                select int.Parse(t);
            IProductService productService = ServiceHelper.Create<IProductService>();
            IOrderService orderService = ServiceHelper.Create<IOrderService>();
            if (integral < 0)
            {
                throw new HimallException("兑换积分数量不正确");
            }
            IEnumerable<string> strs = null;
            if (!string.IsNullOrEmpty(couponIds))
            {
                strs = couponIds.Split(new char[] { ',' });
            }
            IEnumerable<long> nums2 = null;
            if (!string.IsNullOrEmpty(collIds))
            {
                char[] chrArray1 = new char[] { ',' };
                nums2 =
                    from item in collIds.Split(chrArray1)
                    select long.Parse(item);
            }
            if (string.IsNullOrWhiteSpace(skuIds) || string.IsNullOrWhiteSpace(counts))
            {
                throw new HimallException("创建订单的时候，SKU为空，或者数量为0");
            }
            OrderCreateModel orderCreateModel = new OrderCreateModel()
            {
                SkuIds = strArrays1,
                Counts = nums1,
                CurrentUser = base.CurrentUser,
                Integral = integral
            };
            OrderCreateModel orderCreateModel1 = orderCreateModel;
            if (strs == null)
            {
                nums = null;
            }
            else
            {
                nums =
                    from p in strs
                    select long.Parse(p.Split(new char[] { '\u005F' })[0]);
            }
            orderCreateModel1.CouponIds = nums;
            OrderCreateModel orderCreateModel2 = orderCreateModel;
            if (strs == null)
            {
                strArrays = null;
            }
            else
            {
                strArrays =
                    from p in strs
                    select p.Split(new char[] { '\u005F' });
            }
            orderCreateModel2.CouponIdsStr = strArrays;
            orderCreateModel.Invoice = (InvoiceType)invoiceType;
            orderCreateModel.InvoiceTitle = invoiceTitle;
            orderCreateModel.InvoiceContext = invoiceContext;
            orderCreateModel.CollPids = nums2;
            if (strArrays1.Count() == 1)
            {
                string str1 = strArrays1.ElementAt<string>(0);
                if (!string.IsNullOrWhiteSpace(str1))
                {
                    SKUInfo sku = productService.GetSku(str1);
                    bool flag = ServiceHelper.Create<ILimitTimeBuyService>().IsLimitTimeMarketItem(sku.ProductId);
                    orderCreateModel.IslimitBuy = flag;
                }
            }
            orderCreateModel.ReceiveAddressId = recieveAddressId;
            IEnumerable<long> array = (
                from item in orderService.CreateOrder(orderCreateModel)
                select item.Id).ToArray();
            return Json(new { success = true, orderIds = array });
        }

        [HttpPost]
        public JsonResult UpOrderStatus(long orderIds)
        {
            ServiceHelper.Create<IOrderService>().UpdateOrderStatu(orderIds, 2);
            return Json(new { Successful = true });
        }

        [HttpPost]

        public JsonResult AddStatisticsMoney(int TradingType, decimal TradingPrice, string OrderNum, int PayType, string TypeID)
        {
            IStatisticsMoneyService isms = ServiceHelper.Create<IStatisticsMoneyService>();
            decimal moneybynow = isms.GetMoneyByUidType(base.CurrentUser.Id, base.CurrentUser.UserType);
            StatisticsMoney entity = new StatisticsMoney
            {
                UserId = base.CurrentUser.Id,
                UserName = base.CurrentUser.UserName,
                UserType = base.CurrentUser.UserType,
                TradingTime = DateTime.Now,
                TradingType = TradingType,
                TradingPrice = TradingPrice,
                OrderNum = OrderNum,
                PayType = PayType,
                Balance = moneybynow + TradingPrice,
                BalanceAble = moneybynow + TradingPrice
            };
            isms.Add(entity);
            return Json(new { Successful = true });
        }

        public JsonResult UpChargeOrderStatus(long orderIds)
        {
            ServiceHelper.Create<IMemberCapitalService>().UpdateChareeOrderStatu(orderIds);
            return Json(new { Successful = true });
        }
        /// <summary>
        /// 金额计算
        /// </summary>
        /// <param name="Price">变化的金额</param>
        /// <param name="Type">类型add/remove</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Account(decimal Price, string Type)
        {
            #region 获取当前用户的金额信息
            long Id;
            decimal? balance;
            decimal? freezeAmount;
            decimal? chargeAmount;
            decimal? newPrice;
            IMemberCapitalService imcs = ServiceHelper.Create<IMemberCapitalService>();
            CapitalInfo capitalInfo = ServiceHelper.Create<IMemberCapitalService>().GetCapitalInfo(base.CurrentUser.Id);
            if (capitalInfo == null)
            {
                CapitalInfo cinfo = new CapitalInfo
                {
                    MemId = base.CurrentUser.Id,
                    Balance = Price,
                    FreezeAmount = 0,
                    ChargeAmount = 0,
                    ManageId = 0
                };
                imcs.AddCapitalInfo(cinfo);
                Id = cinfo.Id;
                balance = cinfo.Balance;
                freezeAmount = cinfo.FreezeAmount;
                chargeAmount = cinfo.ChargeAmount;
            }
            else
            {
                Id = capitalInfo.Id;
                balance = capitalInfo.Balance;
                freezeAmount = capitalInfo.FreezeAmount;
                chargeAmount = capitalInfo.ChargeAmount;
            }
            #endregion
            if (Type == "add")
            {
                newPrice = balance + Price;
            }
            else
            {
                newPrice = balance - Price;
            }
            imcs.UpdateCapitalAmount(Id, base.CurrentUser.Id, newPrice, 0, 0, 0);
            return Json(new { Successful = true });
        }

        [HttpPost]
        //发送请求支付请求
        public JsonResult SetExpressCheckout(decimal amount, string currency_code, string item_name, string return_false_url, string webtype, string fq = "")
        {
            string hots = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/";
            NVPCodec encoder = new NVPCodec();
            encoder.Add("PAYMENTACTION", "Sale");
            //不允许客户改地址
            //encoder.Add("ADDROVERRIDE", "1");
            encoder.Add("CURRENCYCODE", currency_code);
            encoder.Add("L_NAME0", item_name);
            encoder.Add("L_NUMBER0", item_name);
            encoder.Add("L_DESC0", item_name);
            encoder.Add("L_AMT0", amount.ToString());
            encoder.Add("L_QTY0", "1");
            double ft = double.Parse(amount.ToString());
            encoder.Add("AMT", ft.ToString());
            if (!string.IsNullOrEmpty(webtype))
            {
                encoder.Add("RETURNURL", hots + "/Pay/Return?orderid=" + item_name + "&price=" + amount + "&type=webzf&fq=" + fq + "&paymodel=paypal");
            }
            else
            {
                encoder.Add("RETURNURL", hots + "/Pay/Return?orderid=" + item_name + "&price=" + amount + "&type=webcz&fq=" + fq + "&paymodel=paypal");
            }
            encoder.Add("CANCELURL", return_false_url);
            NVPCodec decoder = PaypalController.SetExpressCheckout(encoder);
            string ack = decoder["ACK"];
            if (!string.IsNullOrEmpty(ack) && (ack.Equals("Success", System.StringComparison.OrdinalIgnoreCase) || ack.Equals("SuccessWithWarning", System.StringComparison.OrdinalIgnoreCase)))
            {
                //Session["TOKEN"] = decoder["token"];
                return Json(ConfigurationManager.AppSettings["RedirectURL"] + decoder["token"]);
            }
            else
            {
                return Json(return_false_url);
            }
        }

        //public JsonResult SetAliPay(decimal amount, string currency_code, string item_name, string return_false_url, string webtype)
        //{
        //    string hots = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/";
        //    ChemCloud.AliPay.AliPay ap = new ChemCloud.AliPay.AliPay();
        //    string key = "";//填写自己的key
        //    string partner = "2088221672323255 ";//填写自己的Partner
        //    ChemCloud.AliPay.StandardGoods bp = new ChemCloud.AliPay.StandardGoods("trade_create_by_buyer", partner, key, "MD5", "卡2", Guid.NewGuid().ToString(), 2.551m, 1, "hao_ding2000@yahoo.com.cn", "hao_ding2000@yahoo.com.cn"
        //        , "EMS", 25.00m, "BUYER_PAY", "1");
        //    //bp.Notify_Url = "http://203.86.79.185/ali/notify.aspx";
        //    if (!string.IsNullOrEmpty(webtype))
        //    {
        //        bp.Return_Url = hots + "/Pay/Return?orderid=" + item_name + "&price=" + amount + "&type=webzf";
        //    }
        //    else
        //    {
        //        bp.Return_Url = hots + "/Pay/Return?orderid=" + item_name + "&price=" + amount + "&type=webcz";
        //    }
        //    return Json(ap.CreateStandardTrade("https://www.alipay.com/cooperate/gateway.do", bp));
        //}


        /// <summary>
        /// SubmitOrder
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult SubmitOrderBatchFun(string json)
        {
            string CoinType = ConfigurationManager.AppSettings["CoinType"] == null ? "1" : ConfigurationManager.AppSettings["CoinType"];
            try
            {
                /*步骤1： 获取物流、开票、收货*/
                //  MargainBill model = Newtonsoft.Json.JsonConvert.DeserializeObject<MargainBill>(json);
                MarginBillModel margin = Newtonsoft.Json.JsonConvert.DeserializeObject<MarginBillModel>(json);
                MargainBill model = null;
                BillShopDeliver shopDeliver = null;
                if (margin != null)
                {
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<MargainBill>(margin.Bill);
                    shopDeliver = Newtonsoft.Json.JsonConvert.DeserializeObject<BillShopDeliver>(margin.Ship);
                }
                if (model != null)
                {
                    /*步骤2：读取购物车*/
                    long num;
                    num = (base.CurrentUser != null ? base.CurrentUser.Id : 0);
                    ShoppingCartInfo cart = ServiceHelper.Create<ICartService>().GetCart(num);
                    IProductService productService = ServiceHelper.Create<IProductService>();
                    IShopService shopService = ServiceHelper.Create<IShopService>();
                    List<ShoppingCartItem> cartlist = cart.Items.ToList(); /*当前用户的购物车产品*/
                    ProductInfo product = new ProductInfo();
                    ShopInfo shop = new ShopInfo();
                    foreach (var item in cartlist)
                    {
                        product = productService.GetProduct(item.ProductId);
                        shop = shopService.GetShop(product.ShopId, false);
                        item.ProductId = product.Id;
                        item.imgUrl = product.ImagePath;
                        item.name = product.ProductName;
                        item.shopName = shop.ShopName;
                        item.productcode = product.ProductCode;
                    }
                    List<ShoppingCartItem> cartshoplist = cartlist.DistinctBy(p => p.ShopId).ToList();
                    /*重构订单参数，来源购物车表*/
                    List<MargainBill> listmarginbill = new List<MargainBill>();
                    MargainBill bill = null;
                    List<ShopDeliver> tmpShopDeliver = null;
                    foreach (ShoppingCartItem cartmodel in cartshoplist)
                    {
                        if (shopDeliver != null && shopDeliver.Shops != null && shopDeliver.Shops.Count > 0)
                        {
                            tmpShopDeliver = shopDeliver.Shops.Where(x => x.ShopId == cartmodel.ShopId).ToList();
                        }
                        bill = new MargainBill();
                        bill.MemberId = num;
                        bill.ShopId = cartmodel.ShopId;
                        bill.RegionId = model.RegionId;
                        if (tmpShopDeliver != null && tmpShopDeliver.Count > 0)
                        {
                            bill.DeliverType = tmpShopDeliver[0].DeliverType;
                            bill.DeliverCost = tmpShopDeliver[0].DeliverCost;
                        }
                        else
                        {
                            bill.DeliverType = model.DeliverType;
                            bill.DeliverCost = model.DeliverCost;
                        }

                        bill.DeliverAddress = model.DeliverAddress;
                        bill.PayMode = model.PayMode;
                        bill.CoinType = long.Parse(CoinType);
                        bill.IsInsurance = model.IsInsurance;
                        bill.Insurancefee = model.Insurancefee;
                        bill.InvoiceType = model.InvoiceType;
                        bill.InvoiceTitle = model.InvoiceTitle;
                        bill.InvoiceContext = model.InvoiceContext;
                        bill.SellerPhone = model.SellerPhone;
                        bill.SellerRemark = model.SellerRemark;
                        bill.SellerAddress = model.SellerAddress;
                        bill.BillStatus = EnumBillStatus.SubmitBargain;
                        bill.CreateDate = DateTime.Now;
                        bill.IsDelete = 0;
                        decimal productamount = 0;
                        List<MargainBillDetail> detaillist = new List<MargainBillDetail>();

                        foreach (ShoppingCartItem cartproduct in cartlist.Where(q => q.ShopId.Equals(cartmodel.ShopId)).ToList())
                        {
                            MargainBillDetail detail = new MargainBillDetail();
                            detail.Id = 1;
                            detail.ProductId = cartproduct.ProductId;
                            detail.ProductName = ServiceHelper.Create<IProductService>().GetProduct(cartproduct.ProductId) == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(cartproduct.ProductId).ProductName;
                            detail.Num = cartproduct.Quantity;
                            detail.PurchasePrice = cartproduct.ProductTotalAmount;
                            detail.PackingUnit = cartproduct.PackingUnit;
                            detail.SpecLevel = cartproduct.SpecLevel;
                            detail.Purity = cartproduct.Purity;
                            detail.CreateDate = DateTime.Now;
                            detail.IsDelete = 0;
                            detail.BillNo = "";
                            detail.MarketPrice = 0;

                            detail.ShopPirce = 0;

                            detail.FinalPrice = 0;
                            detail.Bidder = 1;
                            detail.BidderName = "aa";
                            detail.BuyerMessage = "aa";
                            detail.MessageReply = "aa";
                            detail.CASNo = "";

                            detaillist.Add(detail);
                            productamount = productamount + cartproduct.ProductTotalAmount;
                        }
                        bill.TotalAmount = productamount;
                        bill._MargainBillDetail = detaillist;
                        listmarginbill.Add(bill);
                    }
                    ServiceHelper.Create<IMargainBillService>().CartSubmitOrder(listmarginbill);

                    /*下单发消息提醒供应商*/
                    foreach (ShoppingCartItem cartmodel in cartshoplist)
                    {
                        long userid = ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(cartmodel.ShopId) == null ? 0 :
                         ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(cartmodel.ShopId).Id;
                        string messagecontent = "用户" + base.CurrentUser.UserName + "向你提交了订单，请查看。";
                        ServiceHelper.Create<ISiteMessagesService>().SendSiteMessages(userid, (int)MessageSetting.MessageModuleStatus.OrderCreated, messagecontent, base.CurrentUser.UserName);

                    }

                    return Json(new { success = true, msg = "success！" });
                }
                else
                {
                    return Json(new { success = false, state = 0, msg = "提交失败！" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, state = 1, msg = "提交失败！" });
            }
        }


        [HttpPost]
        [UnAuthorize]
        public JsonResult SubmitOrderAll()
        {
            try
            {
                //1.查询购物车里，所有的产品。
                List<ShoppingCartItem> _ShoppingCarts = ServiceHelper.Create<ICartService>().GetCart(base.CurrentUser.Id).Items.ToList();
                List<ShoppingCartItem> _ordercounts = _ShoppingCarts.DistinctBy(p => new { p.ShopId }).ToList();
                foreach (ShoppingCartItem _ShoppingCartsmodel in _ordercounts)
                {
                    List<ShoppingCartItem> _ShoppingCartsByShopId = new List<ShoppingCartItem>();
                    long shopid = _ShoppingCartsmodel.ShopId;
                    _ShoppingCartsByShopId = _ShoppingCarts.Where(q => q.ShopId == shopid).ToList();

                    MargainBill _MargainBill = new MargainBill();
                    _MargainBill.ShopId = shopid;
                    _MargainBill.RegionId = _ShoppingCartsByShopId.FirstOrDefault().RegionId;
                    _MargainBill.MemberId = base.CurrentUser.Id;
                    _MargainBill.DeliverType = _ShoppingCartsByShopId.FirstOrDefault().ExpressCompanyName;
                    _MargainBill.DeliverCost = _ShoppingCartsByShopId.FirstOrDefault().Freight;
                    _MargainBill.PayMode = _ShoppingCartsByShopId.FirstOrDefault().PaymentTypeName;

                    _MargainBill.CoinType = _ShoppingCartsByShopId.FirstOrDefault().CoinType == "CNY" ? 2 : 1;
                    if (_ShoppingCartsByShopId.FirstOrDefault().Insurancefee > 0)
                    {
                        _MargainBill.IsInsurance = 1;
                        _MargainBill.Insurancefee = _ShoppingCartsByShopId.FirstOrDefault().Insurancefee;
                    }
                    _MargainBill.InvoiceType = _ShoppingCartsByShopId.FirstOrDefault().InvoiceType;
                    _MargainBill.InvoiceTitle = _ShoppingCartsByShopId.FirstOrDefault().InvoiceTitle == null ? "" : _ShoppingCartsByShopId.FirstOrDefault().InvoiceTitle;
                    _MargainBill.InvoiceContext = _ShoppingCartsByShopId.FirstOrDefault().InvoiceContext == null ? "" : _ShoppingCartsByShopId.FirstOrDefault().InvoiceContext;

                    List<MargainBillDetail> list = new List<MargainBillDetail>();
                    decimal ProductTotalAmount = 0;
                    foreach (ShoppingCartItem _ShoppingCartItem in _ShoppingCartsByShopId)
                    {
                        MargainBillDetail model = new MargainBillDetail();
                        model.ProductId = _ShoppingCartItem.ProductId;
                        model.ProductName = ServiceHelper.Create<IProductService>().GetProduct(_ShoppingCartItem.ProductId) == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(_ShoppingCartItem.ProductId).ProductName;
                        model.Num = _ShoppingCartItem.Quantity;
                        model.PurchasePrice = _ShoppingCartItem.ProductTotalAmount;
                        model.PackingUnit = _ShoppingCartItem.PackingUnit;
                        model.SpecLevel = _ShoppingCartItem.SpecLevel;
                        model.Purity = _ShoppingCartItem.Purity;
                        list.Add(model);

                        ProductTotalAmount += _ShoppingCartItem.ProductTotalAmount;
                    };
                    _MargainBill._MargainBillDetail = list;

                    _MargainBill.TotalAmount = ProductTotalAmount;

                    ServiceHelper.Create<IMargainBillService>().SubmitOrderBatch(_MargainBill);
                }

                return Json(new { success = true, msg = "success！" });

            }
            catch (Exception)
            {
                return Json(new { success = false, state = 1, msg = "提交失败！" });
            }
        }

        /// <summary>
        /// 余额支付-给平台
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="payid"></param>
        /// <returns></returns>
        public JsonResult PayToPlatform(string pwd, string totalAmount, string paytype, string id)
        {
            if (ServiceHelper.Create<IMemberCapitalService>().GetMemberInfoByPayPwd(base.CurrentUser.Id, pwd) == null)
            {
                throw new HimallException("支付密码不对");
            }

            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            int m_type = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            Finance_Payment fp = new Finance_Payment();
            fp.PayMent_Number = _orderBO.GenerateOrderNumber();
            fp.PayMent_UserId = base.CurrentUser.Id;
            fp.PayMent_UserType = base.CurrentUser.UserType;
            fp.PayMent_OrderNum = long.Parse(id);
            fp.PayMent_PayTime = DateTime.Now;
            fp.PayMent_PayMoney = decimal.Parse(totalAmount);
            fp.PayMent_TotalMoney = decimal.Parse(totalAmount);
            fp.PayMent_BXMoney = 0;
            fp.PayMent_YFMoney = 0;
            fp.PayMent_JYMoney = 0;
            fp.PayMent_SXMoney = 0;
            fp.PayMent_PayAddress = ChemCloud.Core.Common.GetIpAddress();
            fp.PayMent_MoneyType = m_type;
            fp.PayMent_Status = 1;
            fp.PayMent_Type = int.Parse(paytype);
            ServiceHelper.Create<IFinance_PaymentService>().AddFinance_Payment(fp);


            return Json(new { Successful = true });

        }


        /// <summary>
        /// 支付记录操作完成更新个人钱包
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <param name="price">实际支付的金额</param>
        /// <param name="isWallet">类型 1.是账户余额支付 2.第三方其它支付方式</param>
        /// <returns></returns>
        public JsonResult AddUserPayMentInfo(string orderid, string price, string isWallet, string paytype)
        {
            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            int m_type = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
            Finance_Payment fp = new Finance_Payment();
            fp.PayMent_Number = _orderBO.GenerateOrderNumber();
            fp.PayMent_UserId = base.CurrentUser.Id;
            fp.PayMent_UserType = base.CurrentUser.UserType;
            fp.PayMent_OrderNum = long.Parse(orderid);
            fp.PayMent_PayTime = DateTime.Now;
            fp.PayMent_PayMoney = decimal.Parse(price);
            if (paytype == "0")
            {
                OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
                fp.PayMent_TotalMoney = oinfo.ProductTotalAmount;
                fp.PayMent_BXMoney = oinfo.Insurancefee;
                fp.PayMent_YFMoney = oinfo.Freight;
                fp.PayMent_JYMoney = oinfo.Transactionfee;
                fp.PayMent_SXMoney = oinfo.Counterfee;
            }
            else
            {
                fp.PayMent_TotalMoney = decimal.Parse(price);
                fp.PayMent_BXMoney = 0;
                fp.PayMent_YFMoney = 0;
                fp.PayMent_JYMoney = 0;
                fp.PayMent_SXMoney = 0;
            }
            fp.PayMent_PayAddress = ChemCloud.Core.Common.GetIpAddress();
            fp.PayMent_MoneyType = m_type;
            fp.PayMent_Status = 1;
            fp.PayMent_Type = int.Parse(paytype);
            if (ServiceHelper.Create<IFinance_PaymentService>().AddFinance_Payment(fp))
            {
                Log.Info("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "创建支付记录成功,支付单号：" + fp.PayMent_Number + ".");
                if (isWallet == "1")
                {
                    #region 如果是账户余额支付 更新个人的账户信息
                    Finance_Wallet fw = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, m_type);
                    if (fw != null)
                    {
                        if (fw.Wallet_UserLeftMoney < decimal.Parse(price))
                        {
                            Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "更新自己的账户时出错,出错信息：个人账户额小于订单应支付的金额.");
                            return Json("no");
                        }
                        else
                        {
                            fw.Wallet_DoIpAddress = ChemCloud.Core.Common.GetIpAddress();
                            fw.Wallet_DoLastTime = DateTime.Now;
                            fw.Wallet_DoUserId = base.CurrentUser.Id;
                            fw.Wallet_DoUserName = base.CurrentUser.UserName;
                            fw.Wallet_MoneyType = m_type;
                            fw.Wallet_UserLeftMoney = fw.Wallet_UserLeftMoney - decimal.Parse(price);
                            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fw))
                            {
                                Log.Info("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "成功更新了自己的账户余额.");
                                return Json("ok");
                            }
                            else
                            {
                                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "更新自己的账户时出错.");
                                return Json("no");
                            }
                        }
                    }
                    else
                    {
                        return Json("no");
                    }
                    #endregion
                }
                else
                {
                    return Json("ok");
                }
            }
            else
            {
                Log.Error("用户" + base.CurrentUser.Id + "在" + DateTime.Now + "创建支付记录失败.");
                return Json("no");
            }
        }

        /// <summary>
        /// 余额支付-验证支付密码是否正确
        /// </summary>
        /// <param name="orderIds"></param>
        /// <param name="pwd"></param>
        /// <param name="payid"></param>
        /// <returns></returns>
        public JsonResult PayByCapital(string orderIds, string pwd, string payid)
        {
            /*验证支付密码*/
            //if (ServiceHelper.Create<IMemberCapitalService>().GetMemberInfoByPayPwd(base.CurrentUser.Id, pwd) == null)
            //{
            //    throw new HimallException("支付密码不对");
            //}
            int cointype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CoinType"] == null ? "1" : System.Configuration.ConfigurationManager.AppSettings["CoinType"]);

            if (!ServiceHelper.Create<IFinance_WalletService>().CheckPaymentpassword(base.CurrentUser.Id, pwd, cointype))
            {
                throw new HimallException("支付密码不对");
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 余额支付-验证当前用户的支付密码是否设置
        /// </summary>
        /// <returns></returns>
        public JsonResult IsNullWalletPayPassword()
        {
            int cointype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CoinType"] == null ? "1" : System.Configuration.ConfigurationManager.AppSettings["CoinType"]);

            return Json(new { success = ServiceHelper.Create<IFinance_WalletService>().IsNullWalletPayPassword(base.CurrentUser.Id, cointype) });
        }
    }
}