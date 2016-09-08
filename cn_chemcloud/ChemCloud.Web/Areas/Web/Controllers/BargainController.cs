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
using Newtonsoft.Json;
using ChemColud.Shipping;



namespace ChemCloud.Web.Areas.Web.Controllers
{
    /// <summary>
    /// 议价操作  Bargain
    /// </summary>
    public class BargainController : BaseMemberController
    {
        // GET: Web/Bargain
        public ActionResult Index()
        {
            return View();
        }

        //议价单管理页面
        public ActionResult BargainList()
        {
            return View();
        }

        //议价 详细页面
        public ActionResult BargainDetail(long id)
        {
            string isreview = "false";
            MargainBill _MargainBill = ServiceHelper.Create<IMargainBillService>().GetBillById(id, base.CurrentUser.Id);
            _MargainBill.MemberName = ServiceHelper.Create<IMemberService>().GetMember(_MargainBill.MemberId).RealName == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(_MargainBill.MemberId).RealName;
            _MargainBill.ShopName = ServiceHelper.Create<IShopService>().GetShop(_MargainBill.ShopId).ShopName == null ? "" : ServiceHelper.Create<IShopService>().GetShop(_MargainBill.ShopId).ShopName;
            foreach (var item in _MargainBill._MargainBillDetail)
            {
                item.BidderName = ServiceHelper.Create<IMemberService>().GetMember(item.Bidder) == null ? ServiceHelper.Create<IShopService>().GetShopName(item.Bidder) : ServiceHelper.Create<IMemberService>().GetMember(item.Bidder).RealName;
            }
            if (_MargainBill.BillStatus == EnumBillStatus.Bargaining)
            {
                isreview = "true";
            }
            ViewBag.CurrentUser = base.CurrentUser.Id;
            ViewBag.isreview = isreview;
            if (!string.IsNullOrEmpty(_MargainBill.DeliverAddress) && !"null".Equals(_MargainBill.DeliverAddress))
            {
                ShippingAddressInfo addressinfo = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddress(long.Parse(_MargainBill.DeliverAddress));
                if (addressinfo != null)
                {
                    _MargainBill.DeliverAddress = addressinfo.RegionFullName + ">" + addressinfo.Address;
                }
            }
            else
            {
                _MargainBill.DeliverAddress = "" + ">" + "";
            }
            return View(_MargainBill);
        }

        /// <summary>
        /// 议价单列表查询方法
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="shopname"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult BargainList(int page, int rows, string shopname, string billno)
        {
            BargainBillQuery _BargainBillQuery = new BargainBillQuery()
            {
                PageSize = rows,
                PageNo = page,
                ShopName = shopname,
                MemberId = CurrentUser.Id,
                BillNo = billno
            };

            PageModel<MargainBill> margains = ServiceHelper.Create<IMargainBillService>().GetBill(_BargainBillQuery);
            IEnumerable<MargainBill> array = from item in margains.Models.ToArray()
                                             select new MargainBill
                                             {
                                                 Id = item.Id,
                                                 IsDelete = item.IsDelete,
                                                 strCreateDate = item.CreateDate.ToString("yyyy-MM-dd"),
                                                 BillNo = item.BillNo,
                                                 TotalAmount = item.TotalAmount,
                                                 DeliverType = item.DeliverType,
                                                 strDeliverDate = item.DeliverDate.ToString("yyyy-MM-dd HH"),
                                                 DeliverAddress = item.DeliverAddress,
                                                 DeliverCost = item.DeliverCost,
                                                 PayMode = item.PayMode,
                                                 BillStatus = item.BillStatus,
                                                 MemberId = item.MemberId,
                                                 MemberName = (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).UserName) == null ? "" :
                                                 (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).UserName),
                                                 ShopId = item.ShopId,
                                                 ShopName = (ServiceHelper.Create<IShopService>().GetShop(item.ShopId).ShopName) == null ? "" :
                                                 (ServiceHelper.Create<IShopService>().GetShop(item.ShopId).ShopName),
                                                 CoinType = item.CoinType,
                                                 CoinTypeName = item.CoinTypeName,
                                                 BuyerEmail = (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).Email) == null ? "" : (ServiceHelper.Create<IMemberService>().GetMember(item.MemberId).Email),
                                                 CASNo = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().CASNo,
                                                 ProudctNum = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().Num,
                                                 BuyerMessage = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().BuyerMessage,
                                                 MessageReply = ServiceHelper.Create<IMargainBillService>().GetBillByNo(item.BillNo)._MargainBillDetail.OrderBy(q => q.Id).FirstOrDefault().MessageReply
                                             };
            DataGridModel<MargainBill> dataGridModel = new DataGridModel<MargainBill>()
            {
                rows = array,
                total = margains.Total
            };
            return Json(dataGridModel);
        }

        [HttpPost]
        public JsonResult CloseBargain(string bargainno)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IMargainBillService>().SellerCloseBargain(bargainno);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult BatchCloseBargain(string Ids)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IMargainBillService>().SellerBatchCloseBargain(Ids);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 新增议价单页面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult BargainAdd(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var model = ServiceHelper.Create<IProductService>().GetProduct(long.Parse(Id));
                model.ShopName = ServiceHelper.Create<IShopService>().GetShopName(model.ShopId);
                ViewBag.model = model;
            }

            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            long orderid = _orderBO.GenerateOrderNumber();

            ViewBag.billno = orderid;

            int CoinType = int.Parse(ConfigurationManager.AppSettings["CoinType"] == null ? "1" : ConfigurationManager.AppSettings["CoinType"]);
            List<ProductSpec> listspec = ServiceHelper.Create<IProductService>().GetProductSpec(long.Parse(Id))._ProductSpec.ToList();
            listspec = listspec.Where(q => q.CoinType == CoinType).ToList();

            ViewBag.ProductSpec = listspec;
            ViewBag.userId = base.CurrentUser.Id;

            /*当前货币类型*/
            string cointype = "CNY";
            if (System.Configuration.ConfigurationManager.AppSettings["Language"].ToString() == "2") { cointype = "USD"; }
            ViewBag.cointype = cointype;

            return View(ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id));
        }

        /// <summary>
        /// 产品-供应商页面
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public ActionResult Bargain_SellerList(string productid)
        {
            //根据化学品名称查处同一款产品 对应的供应商和价格
            if (!string.IsNullOrEmpty(productid))
            {
                var model = ServiceHelper.Create<IProductService>().GetProduct(long.Parse(productid));
                ViewBag.ProductInfo = model;
            }

            return View();
        }

        /// <summary>
        /// 产品-供应商查询方法
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="productname"></param>
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
        /// 新增议价单方法
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult AddBargaion(string json)
        {
            string CoinType = ConfigurationManager.AppSettings["CoinType"] == null ? "1" : ConfigurationManager.AppSettings["CoinType"];
            try
            {
                MargainBill model = Newtonsoft.Json.JsonConvert.DeserializeObject<MargainBill>(json);
                if (model != null)
                {
                    model.BillStatus = EnumBillStatus.SubmitBargain;
                    model.CreateDate = DateTime.Now;
                    model.IsDelete = 0;
                    model.MemberId = CurrentUser.Id;
                    model.TotalAmount = 0;
                    model.CoinType = long.Parse(CoinType); //设置币种
                    foreach (var item in model._MargainBillDetail)
                    {
                        item.CreateDate = DateTime.Now;
                        item.IsDelete = 0;
                        model.TotalAmount += item.PurchasePrice;
                        item.Bidder = base.CurrentUser.Id;
                    }
                    model.TotalAmount += model.DeliverCost;
                    ServiceHelper.Create<IMargainBillService>().AddBill(model);

                    long userid = ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(model.ShopId) == null ? 0 : ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(model.ShopId).Id;


                    string messagecontent = "用户" + base.CurrentUser.UserName + "向你提交了询盘，单号：" + model.BillNo + "。请查看。";

                    ServiceHelper.Create<ISiteMessagesService>().SendSiteMessages(userid, (int)MessageSetting.MessageModuleStatus.XunPan, messagecontent, base.CurrentUser.UserName);

                    return Json(new { success = true, msg = "添加成功！" });
                }
                else
                {
                    return Json(new { success = false, msg = "添加失败！" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "添加失败！" });
            }
        }

        /// <summary>
        /// 修改议价单方法
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult UpdateBargaion(long Id, int Num, decimal PurchasePrice, string BuyerMessage)
        {
            try
            {
                MargainBill model = ServiceHelper.Create<IMargainBillService>().GetBillById(Id, base.CurrentUser.Id);
                if (model != null)
                {
                    MargainBillDetail modeldetail = model._MargainBillDetail.OrderByDescending(q => q.Id).FirstOrDefault();
                    MargainBillDetail detail = new MargainBillDetail();
                    detail.CreateDate = DateTime.Now;
                    detail.IsDelete = 0;
                    detail.BillNo = modeldetail.BillNo;
                    detail.ProductId = modeldetail.ProductId;
                    detail.ProductName = modeldetail.ProductName;
                    detail.MarketPrice = modeldetail.MarketPrice;
                    detail.PackingUnit = modeldetail.PackingUnit;
                    detail.Purity = modeldetail.Purity;
                    detail.Num = Num;
                    detail.PurchasePrice = PurchasePrice;
                    detail.SpecLevel = modeldetail.SpecLevel;
                    detail.Bidder = base.CurrentUser.Id;
                    detail.CASNo = modeldetail.CASNo;
                    detail.BuyerMessage = BuyerMessage;
                    ServiceHelper.Create<IMargainBillService>().AddMargainDetail(detail);

                    ServiceHelper.Create<IMargainBillService>().UpdateBillStatu(model.Id, EnumBillStatus.SubmitBargain);//采购商再次询盘时，重置询盘状态为：已提交（未回复）

                    return Json(new { success = true, msg = "修改成功！" });
                }
                else
                {
                    return Json(new { success = false, msg = "修改失败！" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "修改失败！" });
            }
        }


        /// <summary>
        /// 结束议价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult OverBargain(long Id)
        {
            try
            {
                ServiceHelper.Create<IMargainBillService>().OverBargain(Id);
                return Json(new { success = true, msg = "成功！" });
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "失败！" });
            }
        }


        /// <summary>
        /// 下单页面
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult SubmitOrder(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var model = ServiceHelper.Create<IProductService>().GetProduct(long.Parse(Id));
                model.ShopName = ServiceHelper.Create<IShopService>().GetShopName(model.ShopId);
                ViewBag.model = model;
            }

            ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
            long orderid = _orderBO.GenerateOrderNumber();

            ViewBag.billno = orderid;

            int CoinType = int.Parse(ConfigurationManager.AppSettings["CoinType"] == null ? "1" : ConfigurationManager.AppSettings["CoinType"]);
            List<ProductSpec> listspec = ServiceHelper.Create<IProductService>().GetProductSpec(long.Parse(Id))._ProductSpec.ToList();
            listspec = listspec.Where(q => q.CoinType == CoinType).ToList();

            ViewBag.ProductSpec = listspec;

            ViewBag.ProConType = ConfigurationManager.AppSettings["CoinType"];

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

            IOrderService orderService = ServiceHelper.Create<IOrderService>();
            ViewBag.InvoiceTitle = orderService.GetInvoiceTitles(base.CurrentUser.Id); //发票单位
            ViewBag.InvoiceContext = orderService.GetInvoiceContexts();//发票内容
            ViewBag.orderAddressInfo = ServiceHelper.Create<IorderAddressService>().GetUserorderAddressByUserId(base.CurrentUser.Id);
            /*判断是否已经添加了购物地址*/
            string iseditshippingaddress = "0";
            List<ShippingAddressInfo> item = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id).ToList();
            if (item.Count > 0)
            {
                iseditshippingaddress = "1";
            }
            ViewBag.iseditshippingaddress = iseditshippingaddress;

            return View(ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id));
        }


        /// <summary>
        /// SubmitOrder
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [UnAuthorize]
        public JsonResult SubmitOrderFun(string json)
        {
            //1：CNY 2：USD
            string CoinType = ConfigurationManager.AppSettings["CoinType"] == null ? "2" : ConfigurationManager.AppSettings["CoinType"];
            try
            {
                MarginBillModel margin = Newtonsoft.Json.JsonConvert.DeserializeObject<MarginBillModel>(json);
                // MargainBill model = Newtonsoft.Json.JsonConvert.DeserializeObject<MargainBill>(json);
                MargainBill model = null;
                ShipmentEx ship = null;
                if (margin != null)
                {
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<MargainBill>(margin.Bill);
                    ship = Newtonsoft.Json.JsonConvert.DeserializeObject<ShipmentEx>(margin.Ship);
                }

                if (model != null)
                {
                    //if (model.DeliverDate.Date < DateTime.Now.Date)
                    //{
                    //    return Json(new { success = false, state = 1, msg = "发货时间在今天之前！" });
                    //}
                    model.BillStatus = EnumBillStatus.SubmitBargain;
                    model.CreateDate = DateTime.Now;
                    model.IsDelete = 0;
                    model.MemberId = CurrentUser.Id;
                    model.TotalAmount = 0;
                    foreach (var item in model._MargainBillDetail)
                    {
                        item.CreateDate = DateTime.Now;
                        item.IsDelete = 0;
                        model.TotalAmount += item.PurchasePrice;
                        item.Bidder = base.CurrentUser.Id;
                    }

                    model.CoinType = long.Parse(CoinType);

                    ServiceHelper.Create<IMargainBillService>().SubmitOrder(model, ship);

                    long userid = ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(model.ShopId) == null ? 0 :
                        ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(model.ShopId).Id;

                    string messagecontent = "用户" + base.CurrentUser.UserName + "向你提交了订单，单号：" + model.BillNo + "。请查看。";
                    ServiceHelper.Create<ISiteMessagesService>().SendSiteMessages(userid, (int)MessageSetting.MessageModuleStatus.OrderCreated, messagecontent, base.CurrentUser.UserName);

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



        /// <summary>
        /// 新增关注
        /// </summary>
        /// <param name="ShopId"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAttention(long UserId, long ShopId, long ProductId)
        {
            bool ShopSucess = ServiceHelper.Create<IAttentionService>().AddAttention(UserId, ShopId, ProductId);
            if (ShopSucess)
                return Json(new { success = true, msg = "关注成功！" });
            else
                return Json(new { success = false, msg = "您已经关注过这个供应商的该产品！" });
        }

        [HttpPost]
        public JsonResult SubmitLimit(string json)
        {
            string CoinType = ConfigurationManager.AppSettings["CoinType"] == null ? "2" : ConfigurationManager.AppSettings["CoinType"];
            try
            {
                MargainBill model = Newtonsoft.Json.JsonConvert.DeserializeObject<MargainBill>(json);

                if (model != null)
                {
                    model.BillStatus = EnumBillStatus.SubmitBargain;
                    model.CreateDate = DateTime.Now;
                    model.IsDelete = 0;
                    model.MemberId = CurrentUser.Id;
                    model.TotalAmount = 0;
                    foreach (var item in model._MargainBillDetail)
                    {
                        item.CreateDate = DateTime.Now;
                        item.IsDelete = 0;
                        model.TotalAmount += item.PurchasePrice;
                        item.Bidder = base.CurrentUser.Id;
                    }
                    //model.TotalAmount += model.DeliverCost;
                    model.CoinType = long.Parse(CoinType);
                    //曹凯添加方法
                    //ApplyAmountInfo Applymodel = new ApplyAmountInfo()
                    //{
                    //    ApplyUserId = base.CurrentUser.Id,
                    //    ApplyAmount = model.TotalAmount,
                    //    ApplyDate = DateTime.Now,
                    //    CoinType = int.Parse(model.CoinType.ToString()),
                    //    OrderId = long.Parse(model.BillNo)
                    //};
                    //ServiceHelper.Create<IApplyAmountService>().AddApplyAmount(Applymodel);

                }
                return Json(new { success = true, msg = "提交成功！" });
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "提交失败！" });
            }
        }

        [HttpPost]
        public JsonResult GetAddress()
        {
            List<ShippingAddressInfo> item = ServiceHelper.Create<IShippingAddressService>().GetUserShippingAddressByUserId(base.CurrentUser.Id).ToList();
            if (item.Count > 0)
            {
                List<AddressInfos> list = new List<AddressInfos>();
                foreach (var iitem in item)
                {
                    AddressInfos modell = new AddressInfos();
                    modell.Id = iitem.Id;
                    modell.RegionFullName = iitem.RegionFullName;
                    modell.Address = iitem.Address;
                    list.Add(modell);
                }

                string jsonstr = JsonConvert.SerializeObject(list);
                return Json(new { success = true, msg = jsonstr });
            }
            else
            {
                return Json(new { success = false, msg = "无可用收货地址！" });
            }
        }

    }


    public class AddressInfos
    {
        public long Id { get; set; }
        public string RegionFullName { get; set; }
        public string Address { get; set; }
    }

}


