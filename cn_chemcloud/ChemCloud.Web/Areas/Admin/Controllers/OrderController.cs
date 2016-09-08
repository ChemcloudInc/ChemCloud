using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using ChemColud.Shipping;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        public OrderController()
        {
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult CloseOrder(long orderId)
        {
            Result result = new Result();
            ServiceHelper.Create<IOrderService>().PlatformCloseOrder(orderId, base.CurrentManager.UserName, "");
            result.success = true;
            return Json(result);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult ConfirmPay(long orderId, string payRemark)
        {
            Result result = new Result();
            ServiceHelper.Create<IOrderService>().PlatformConfirmOrderPay(orderId, payRemark, base.CurrentManager.UserName);
            result.success = true;
            return Json(result);
        }

        [HttpPost]
        public ActionResult DeleteInvoiceContexts(long id)
        {
            ServiceHelper.Create<IOrderService>().DeleteInvoiceContext(id);
            return Json(true);
        }

        public ActionResult Detail(long id)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(id);
            ViewBag.Coupon = 0;
            foreach (var r in order.OrderItemInfo)
            {
                r.ImagePath = ServiceHelper.Create<IProductService>().GetProduct(r.ProductId) == null ? "" : (ServiceHelper.Create<IProductService>().GetProduct(r.ProductId).ImagePath == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(r.ProductId).ImagePath);
                r.ThumbnailsUrl = ServiceHelper.Create<IProductService>().GetProduct(r.ProductId) == null ? "" : (ServiceHelper.Create<IProductService>().GetProduct(r.ProductId).STRUCTURE_2D == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(r.ProductId).STRUCTURE_2D);
            }

            ViewBag.COA = "";
            if (ServiceHelper.Create<IChemCloud_OrderWithCoaService>().GetChemCloud_OrderWithCoaByOrderid(id) != null)
            {
                string strcoa = ServiceHelper.Create<IChemCloud_OrderWithCoaService>().GetChemCloud_OrderWithCoaByOrderid(id).CoaNo;
                if (!string.IsNullOrEmpty(strcoa))
                {
                    COAList Coa = ServiceHelper.Create<ICOAListService>().GetCoAReportInfo(strcoa);
                    if (Coa != null)
                    {
                        string coaurl = ChemCloud.Core.Common.GetRootUrl("") + "/search/Search_COA?key=" + Coa.CoANo;
                        ViewBag.COA = coaurl;
                    }
                }
            }

            return View(order);
        }


        public ActionResult BehalfShip(long id)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(id);
            return View(order);
        }


        [HttpPost]
        [UnAuthorize]
        public JsonResult GetExpressData(string shipOrderNumber)
        {
            OrderExpressQuery oe = ServiceHelper.Create<IOrderExpressQueryService>().GetOrderExpressById(shipOrderNumber);
            return Json(oe);
            //string end = "暂时没有此快递单号的信息";
            //if (string.IsNullOrEmpty(expressCompanyName) || string.IsNullOrEmpty(shipOrderNumber))
            //{
            //    return Json(end);
            //}
            //string kuaidi100Code = ServiceHelper.Create<IExpressService>().GetExpress(expressCompanyName).Kuaidi100Code;
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://www.kuaidi100.com/query?type={0}&postid={1}", kuaidi100Code, shipOrderNumber));
            //httpWebRequest.Timeout = 8000;
            //HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    Stream responseStream = response.GetResponseStream();
            //    StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
            //    end = streamReader.ReadToEnd();
            //    end = end.Replace("&amp;", "");
            //    end = end.Replace("&nbsp;", "");
            //    end = end.Replace("&", "");
            //}
            //return Json(end);
        }

        private string GetIconSrc(PlatformType platform)
        {
            if (platform == PlatformType.IOS || platform == PlatformType.Android)
            {
                return "/images/app.png";
            }
            return string.Format("/images/{0}.png", platform.ToString());
        }

        [HttpPost]
        public JsonResult GetInvoiceContexts()
        {
            List<InvoiceContextInfo> list = ServiceHelper.Create<IOrderService>().GetInvoiceContexts().ToList();
            return Json(new { rows = list, total = list.Count() });
        }

        public ActionResult Print(long orderId, string shopName)
        {
            OrderItemInfo orderInfo = ServiceHelper.Create<IOrderService>().GetOrderItemInfo(orderId);
            ProductInfo productInfo = ServiceHelper.Create<IProductService>().GetProduct(orderInfo.ProductId);
            CASInfo casInfo = ServiceHelper.Create<ICASInfoService>().GetCASByNo(productInfo.CASNo);
            ShopInfo _shopinfo = ServiceHelper.Create<IShopService>().GetShop(orderInfo.ShopId);
            ViewBag.userName = _shopinfo.ContactsName;
            ViewBag.tel = _shopinfo.ContactsPhone;
            ViewBag.fax = _shopinfo.Fax;
            ViewBag.phone = _shopinfo.ContactsPhone;
            ViewBag.email = _shopinfo.ContactsEmail;
            ViewBag.address = _shopinfo.CompanyAddress;
            ViewBag.url = "https://pubchem.ncbi.nlm.nih.gov/image/imgsrv.fcgi?cid=" + casInfo.Pub_CID + "&t=l";
            ViewBag.shopName = shopName;
            return View(productInfo);
        }

        public ActionResult PrintProductCertification(long orderId, string shopName)
        {
            OrderItemInfo orderInfo = ServiceHelper.Create<IOrderService>().GetOrderItemInfo(orderId);
            ProductInfo productInfo = ServiceHelper.Create<IProductService>().GetProduct(orderInfo.ProductId);
            ViewBag.shopName = shopName;
            return View(productInfo);
        }

        public long GenerateInvoiceNumber()
        {
            long num;
            string empty = string.Empty;
            Guid guid = Guid.NewGuid();
            Random random = new Random(BitConverter.ToInt32(guid.ToByteArray(), 0));
            for (int i = 0; i < 5; i++)
            {
                int num1 = random.Next();
                char chr = (char)(48 + (ushort)(num1 % 10));
                empty = string.Concat(empty, chr.ToString());
            }
            DateTime now = DateTime.Now;
            num = long.Parse(string.Concat(now.ToString("yyyyMMddfff"), empty));
            return num;
        }

        public ActionResult InvoiceContext()
        {
            return View();
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult List(DateTime? startDate, DateTime? endDate, long? orderId, int? orderStatus, string shopName, string userName, string paymentTypeGateway, int page, int rows)
        {
            OrderInfo.OrderOperateStatus? nullable;
            OrderQuery orderQuery = new OrderQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                OrderId = orderId
            };
            OrderQuery orderQuery1 = orderQuery;
            int? nullable1 = orderStatus;
            if (nullable1.HasValue)
            {
                nullable = new OrderInfo.OrderOperateStatus?((OrderInfo.OrderOperateStatus)nullable1.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            orderQuery1.Status = nullable;
            orderQuery1.ShopName = shopName;
            orderQuery1.UserName = userName;
            orderQuery1.PaymentTypeGateway = paymentTypeGateway;
            orderQuery1.PageSize = rows;
            orderQuery1.PageNo = page;
            PageModel<OrderInfo> orders = ServiceHelper.Create<IOrderService>().GetOrders<OrderInfo>(orderQuery1, null);
            IEnumerable<OrderModel> array =
                from item in orders.Models.ToArray()
                select new OrderModel()
                {
                    OrderId = item.Id,
                    OrderStatus = item.OrderStatus.ToDescription(),
                    OrderDate = item.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ShopId = item.ShopId,
                    ShopName = item.ShopName,
                    CompanyName = item.CompanyName,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    TotalPrice = item.OrderTotalAmount,
                    PaymentTypeName = item.PaymentTypeName,
                    PlatForm = (int)item.Platform,
                    IconSrc = GetIconSrc(item.Platform),
                    PlatformText = item.Platform.ToDescription(),
                    SellerRemark = item.SellerRemark,

                    IsBehalfShip = item.IsBehalfShip,
                    BehalfShipType = item.BehalfShipType,
                    BehalfShipNumber = item.BehalfShipNumber
                };
            DataGridModel<OrderModel> dataGridModel = new DataGridModel<OrderModel>()
            {
                rows = array,
                total = orders.Total
            };
            return Json(dataGridModel);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult ListShip(DateTime? startDate, DateTime? endDate, long? orderId, int? orderStatus, string shopName, string userName, string paymentTypeGateway, int page, int rows)
        {
            OrderInfo.OrderOperateStatus? nullable;
            OrderQuery orderQuery = new OrderQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                OrderId = orderId
            };
            OrderQuery orderQuery1 = orderQuery;
            int? nullable1 = orderStatus;

            //if (nullable1.HasValue)
            //{
            //    nullable = new OrderInfo.OrderOperateStatus?((OrderInfo.OrderOperateStatus)nullable1.GetValueOrDefault());
            //}
            //else
            //{
            //    nullable = null;
            //}
            if (nullable1.HasValue)
            {
                orderQuery1.ProcessStatus = orderStatus.ToString();
            }

            //orderQuery1.Status = nullable;
            orderQuery1.ShopName = shopName;
            orderQuery1.UserName = userName;
            orderQuery1.PaymentTypeGateway = paymentTypeGateway;
            orderQuery1.PageSize = rows;
            orderQuery1.PageNo = page;
            PageModel<OrderInfo> orders = ServiceHelper.Create<IOrderService>().GetOrdersAdv<OrderInfo>(orderQuery1, null);
            // orders.Models = orders.Models.Where(x=>x.IsBehalfShip=="1");
            IEnumerable<OrderModel> array =
                from item in orders.Models.ToArray()
                select new OrderModel()
                {
                    OrderId = item.Id,
                    OrderStatus = item.OrderStatus.ToDescription(),
                    OrderDate = item.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ShopId = item.ShopId,
                    ShopName = item.ShopName,
                    CompanyName = item.CompanyName,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    TotalPrice = item.OrderTotalAmount,
                    PaymentTypeName = item.PaymentTypeName,
                    PlatForm = (int)item.Platform,
                    IconSrc = GetIconSrc(item.Platform),
                    PlatformText = item.Platform.ToDescription(),
                    SellerRemark = item.SellerRemark,

                    IsBehalfShip = item.IsBehalfShip,
                    BehalfShipType = item.BehalfShipType,
                    BehalfShipNumber = item.BehalfShipNumber
                };
            DataGridModel<OrderModel> dataGridModel = new DataGridModel<OrderModel>()
            {
                rows = array,
                total = orders.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult PrintShipOrder(string orderid)
        {
            long orderId = long.Parse(orderid);
            List<OrderShip> orderShipList = ServiceHelper.Create<IShipmentService>().GetOrderShip(orderId);


            return View(orderShipList);
        }

        public ActionResult NewPdfPrint(string orderShipId)
        {
            if (!string.IsNullOrEmpty(orderShipId))
            {
                long id = long.Parse(orderShipId);
                OrderShip orderShip = ServiceHelper.Create<IShipmentService>().GetShipOrder(id);
                string base64PDF = string.Empty;
                if (orderShip != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", orderShip.LabelImage.Length.ToString());
                    Response.BinaryWrite(orderShip.LabelImage);
                }
            }
            return View("");
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult CreateShipInfo(string orderid)
        {

            ShipReply reply = null;

            try
            {
                long orderId = long.Parse(orderid);
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(orderId);

                ISiteSettingService siteService = ServiceHelper.Create<ISiteSettingService>();
                string FedExKey = siteService.GetSiteValue("FedExKey");
                string FedExPassword = siteService.GetSiteValue("FedExPassword");
                string FedExAccountNumber = siteService.GetSiteValue("FedExAccountNumber");
                string FedExMeterNumber = siteService.GetSiteValue("FedExMeterNumber");
                string FedShipUrl = siteService.GetSiteValue("FedShipUrl");

                string FedPersonName = siteService.GetSiteValue("PlatformCollectionPersonName");
                string FedCompanyName = siteService.GetSiteValue("PlatformCollectionCompanyName");
                string FedPhoneNumber = siteService.GetSiteValue("PlatformCollectionPhoneNumber");
                string FedStreetLine = siteService.GetSiteValue("PlatformCollectionStreetLine");
                string FedCity = siteService.GetSiteValue("PlatformCollectionCity");
                string FedPostalCode = siteService.GetSiteValue("PlatformCollectionPostalCode");
                string FedCountryCode = siteService.GetSiteValue("PlatformCollectionCountryCode");

                IShipmentService shipmentService = ServiceHelper.Create<IShipmentService>();

                Address origin = new Address(FedStreetLine, "", "", FedCity, "", FedPostalCode, FedCountryCode);
                origin.PersonName = FedPersonName;
                origin.PhoneNumber = FedPhoneNumber;
                origin.CompanyName = FedCompanyName;

                Address dest = shipmentService.GetAddress(order.UserId, order.RegionId);

                if (dest != null)
                {
                    dest.Line1 = order.Address;
                    dest.PersonName = order.ShipTo;
                    dest.PhoneNumber = order.CellPhone;
                }
                else {
                    dest = new Address(order.Address, null, null, null, null, null, null);
                    dest.PersonName = order.ShipTo;
                    dest.PhoneNumber = order.CellPhone;
                }

                List<OrderItemInfo> itemInfoList = order.OrderItemInfo.OrderBy(x => x.Id).ToList();
                if (itemInfoList != null && itemInfoList.Count > 0)
                {
                    List<ShipPackage> pkgList = new List<ShipPackage>();
                    foreach (OrderItemInfo item in itemInfoList)
                    {
                        ShipPackage package = new ShipPackage() { Num = Convert.ToInt32(item.Quantity), PackingUnit = item.PackingUnit };
                        package.OrderItemId = item.Id;
                        package.Description = string.Format("{0}-{1}", item.ProductName, item.SpecLevel);
                        pkgList.Add(package);
                    }

                    CreateShipQuery query = new CreateShipQuery() { Url = FedShipUrl, Key = FedExKey, Password = FedExPassword, AccountNumber = FedExAccountNumber, MeterNumber = FedExMeterNumber };
                    query.ShipPkgs = pkgList;
                    query.Origin = origin;
                    query.Dest = dest;
                    query.CoinType = order.CoinType == 1 ? "CNY" : "USD";
                    query.OrderId = orderId;

                    reply = shipmentService.CreateShip(query);
                }

                if (reply != null)
                {
                    if (!String.IsNullOrEmpty(reply.TrackNumber))
                    {
                        return Json(new { Success = true, TrackNumber = reply.TrackNumber });
                    }
                    else
                    {
                        return Json(new { TrackNumber = "", msg = reply.ErrorMessage });
                    }
                }
                else
                {
                    return Json(new { TrackNumber = "" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message });

            }


        }

        //发货
        [HttpPost]
        [UnAuthorize]
        public JsonResult DeliverGoods(string orderId, string companyName, string shipOrderNumber)
        {
            Result result = new Result();
            try
            {
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderId));
                if (order != null)
                {
                    // order.ExpressCompanyName = companyName;
                    // order.ShipOrderNumber = shipOrderNumber;

                    order.ShippingDate = DateTime.Now;
                    order.BehalfShipNumber = shipOrderNumber;
                    order.BehalfShipType = companyName;
                    order.OrderStatus = OrderInfo.OrderOperateStatus.WaitReceiving;
                }
                result.success = ServiceHelper.Create<IOrderService>().UpdateShip(order);
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }

            return Json(result);
        }

        public ActionResult CreateShip(string orderid)
        {

            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));

             
            return View(order);
        }


        public ActionResult Management()
        {
            IEnumerable<PluginInfo> plugins =
                from t in PluginsManagement.GetPlugins<IPaymentPlugin>()
                select t.PluginInfo;
            return View(plugins);
        }

        public ActionResult ManageShip()
        {
            IEnumerable<PluginInfo> plugins =
                from t in PluginsManagement.GetPlugins<IPaymentPlugin>()
                select t.PluginInfo;
            return View(plugins);
        }


        [HttpPost]
        public ActionResult SaveInvoiceContext(string name, long id = -1L)
        {
            InvoiceContextInfo invoiceContextInfo = new InvoiceContextInfo()
            {
                Id = id,
                Name = name
            };
            ServiceHelper.Create<IOrderService>().SaveInvoiceContext(invoiceContextInfo);
            return Json(true);
        }

        /// <summary>
        /// 平台更新订单的物流单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="wlnum"></param>
        /// <returns></returns>
        public JsonResult AddWLInfo(string orderId, string wlnum)
        {
            if (string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(wlnum))
            {
                return Json("no");
            }
            OrderInfo oinf = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderId));
            if (oinf == null)
            {
                return Json("no");
            }
            oinf.SellerRemark = "ok";
            oinf.ShipOrderNumber = wlnum;
            if (ServiceHelper.Create<IOrderService>().UpdateOrderShipNum(oinf))
            {
                return Json("yes");
            }
            else
            {
                return Json("no");
            }
        }

        /// <summary>
        /// 打印弹出页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewPrintOrder(string orderid)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));

            foreach (var r in order.OrderItemInfo)
            {
                r.Id = r.Id;
                r.CASNo = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).CASNo : "";
                r.ThumbnailsUrl = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).ImagePath : "";
                r.MolecularFormula = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).MolecularFormula : "";

            }
            if (order != null && !string.IsNullOrEmpty(order.ShipOrderNumber))
            {
                Code128 _Code = new Code128();
                System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(order.ShipOrderNumber, Code128.Encode.Code128A);
                string ImagePath = System.AppDomain.CurrentDomain.BaseDirectory + "Storage/Code128/";

                if (!Directory.Exists(ImagePath))
                {
                    Directory.CreateDirectory(ImagePath);
                }
                string path = order.ShipOrderNumber + DateTime.Now.ToString("yyyyMMddHHmmss") + ".gif";
                ImagePath = ImagePath + path;

                imgTemp.Save(ImagePath, System.Drawing.Imaging.ImageFormat.Gif);
                ViewBag.CodePath = ChemCloud.Core.Common.GetRootUrl("") + "/Storage/Code128/" + path;
            }
            else
            {
                ViewBag.CodePath = "";
            }
            return View(order);
        }

        /// <summary>
        /// 物流面单打印
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ActionResult NewPrintExpress(string orderid)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));

            foreach (var r in order.OrderItemInfo)
            {
                r.Id = r.Id;
                r.CASNo = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).CASNo : "";
                r.ThumbnailsUrl = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).ImagePath : "";
                r.MolecularFormula = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).MolecularFormula : "";

            }

            if (order != null && !string.IsNullOrEmpty(order.ShipOrderNumber))
            {
                //条形码
                Code128 _Code = new Code128();
                System.Drawing.Bitmap imgTemp = _Code.GetCodeImage(order.ShipOrderNumber, Code128.Encode.Code128A);
                string ImagePath = System.AppDomain.CurrentDomain.BaseDirectory + "Storage/Code128/";

                if (!Directory.Exists(ImagePath))
                {
                    Directory.CreateDirectory(ImagePath);
                }
                string path = order.ShipOrderNumber + DateTime.Now.ToString("yyyyMMddHHmmss") + ".gif";
                ImagePath = ImagePath + path;

                imgTemp.Save(ImagePath, System.Drawing.Imaging.ImageFormat.Gif);
                ViewBag.CodePath = ChemCloud.Core.Common.GetRootUrl("") + "/Storage/Code128/" + path;

                if (ServiceHelper.Create<IChemCloud_OrderWithCoaService>().GetChemCloud_OrderWithCoaByOrderid(long.Parse(orderid)) == null) { ViewBag.QRcode = null; }
                else
                {
                    //二维码
                    string CoaNo = ServiceHelper.Create<IChemCloud_OrderWithCoaService>().GetChemCloud_OrderWithCoaByOrderid(long.Parse(orderid)).CoaNo;
                    if (!string.IsNullOrEmpty(CoaNo))
                    {
                        CoaNo = ChemCloud.Core.Common.GetRootUrl("") + "/search/Search_COA?key=" + CoaNo;
                        QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                        qrCodeEncoder.QRCodeScale = 2;
                        qrCodeEncoder.QRCodeVersion = 6;
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        System.Drawing.Image image = qrCodeEncoder.Encode(CoaNo);//二维码内容
                        string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
                        string filepath = Server.MapPath(@"~\Temp\") + filename;
                        System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                        image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        fs.Close();
                        image.Dispose();

                        ViewBag.QRcode = ChemCloud.Core.Common.GetRootUrl("") + "/Temp/" + filename;

                        //if (System.IO.File.Exists(filepath))
                        //{
                        //    //上传到服务器后 删除本地文件
                        //    System.IO.File.Delete(filepath);
                        //}
                    }
                    else { ViewBag.QRcode = null; }
                }
            }
            else
            {
                ViewBag.CodePath = "";
            }

            return View(order);
        }

        //发票打印
        public ActionResult PrintInvoice(string ids)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            long id = nums.FirstOrDefault();

  
            long[] array = (
                from item in ids.Split(chrArray)
                select long.Parse(item)).ToArray();

            OrderInfo orderInfo = ServiceHelper.Create<IOrderService>().GetOrder(id);

            List<OrderItemInfo> orderItemInfo = ServiceHelper.Create<IOrderService>().GetOrdersByIds(array);
            decimal total = 0;
            foreach (OrderItemInfo list in orderItemInfo)
            {
                total = total + list.SalePrice;
            }
            ViewBag.email = ServiceHelper.Create<IMemberService>().GetMember(orderInfo.UserId).Email;
            ViewBag.invoice = GenerateInvoiceNumber();
            ViewBag.strDate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.total = total;

            ViewBag.orderItemInfo = orderItemInfo;

            long shopid = orderInfo.ShopId;
            ShopInfo shopinfo = ServiceHelper.Create<IShopService>().GetShop(shopid);
            if (shopinfo != null)
            {
                ViewBag.BankName = shopinfo.BankName;
                ViewBag.BankAccountName = shopinfo.BankAccountName;
                ViewBag.BankAccountNumber = shopinfo.BankAccountNumber;
            }
            else
            {
                ViewBag.BankName = "";
                ViewBag.BankAccountName = "";
                ViewBag.BankAccountNumber = "";
            }


            return View(orderInfo);
 
        }

        public ActionResult PrintInvoiceShip(string ids)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            long id = nums.FirstOrDefault();

 
            long[] array = (
                from item in ids.Split(chrArray)
                select long.Parse(item)).ToArray();

            OrderInfo orderInfo = ServiceHelper.Create<IOrderService>().GetOrder(id);

            List<OrderItemInfo> orderItemInfo = ServiceHelper.Create<IOrderService>().GetOrdersByIds(array);
       
            ViewBag.email = ServiceHelper.Create<IMemberService>().GetMember(orderInfo.UserId).Email;
            ViewBag.invoice = GenerateInvoiceNumber();
            ViewBag.strDate = DateTime.Now.ToString("MM/dd/yyyy");
 
            ViewBag.orderItemInfo = orderItemInfo;


            long shopid = orderInfo.ShopId;
            ShopInfo shopinfo = ServiceHelper.Create<IShopService>().GetShop(shopid);
            if (shopinfo != null)
            {
                ViewBag.BankName = shopinfo.BankName;
                ViewBag.BankAccountName = shopinfo.BankAccountName;
                ViewBag.BankAccountNumber = shopinfo.BankAccountNumber;
            }
            else
            {
                ViewBag.BankName = "";
                ViewBag.BankAccountName = "";
                ViewBag.BankAccountNumber = "";
            }

            return View(orderInfo);
        }

    }
}