using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using ChemCloud.DBUtility;


namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class OrderController : BaseSellerController
    {
        public OrderController()
        {
        }

        /// <summary>
        /// 供应商设置订单为平台代发货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BehalfShip(string orderid)
        {
            string strsql = string.Format("UPDATE Chemcloud_Orders SET IsBehalfShip=1 WHERE Id='{0}'", orderid);
            int result = DbHelperSQL.ExecuteSql(strsql);

            if (result > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        [ShopOperationLog(Message = "商家取消订单")]
        public JsonResult CloseOrder(long orderId)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IOrderService>().SellerCloseOrder(orderId, base.CurrentSellerManager.UserName);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ConfirmSendGood(string ids, string companyNames, string shipOrderNumbers, string coaid, int isreplacedeliver, string replacedeliveraddress)
        {
            coaid = coaid.Substring(0, coaid.Length - 1);
            coaid = coaid + "]";
            List<Coaid> coalist = JsonConvert.DeserializeObject<List<Coaid>>(coaid);

            Result result = new Result();

            try
            {
                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from item in ids.Split(chrArray)
                    select long.Parse(item);
                string[] strArrays = companyNames.Split(new char[] { ',' });
                string[] strArrays1 = shipOrderNumbers.Split(new char[] { ',' });
                ServiceHelper.Create<IOrderService>().GetOrders(nums);
                int num = 0;
                foreach (long num1 in nums)
                {
                    ServiceHelper.Create<IOrderService>().SellerSendGood(num1, base.CurrentSellerManager.UserName, strArrays[num], strArrays1[num], isreplacedeliver, replacedeliveraddress);
                    #region 物流通关消息发送 xzg
                    IOrderService IOS = ServiceHelper.Create<IOrderService>();
                    OrderInfo OINFO = IOS.GetOrder(num1);
                    if (OINFO != null)
                    {
                        ServiceHelper.Create<ISiteMessagesService>().SendLogisticsClearanceMessage(OINFO.UserId, num1.ToString());
                    }

                    foreach (Coaid item in coalist)
                    {
                        if (item.coaNo == "请选择")
                        {
                            continue;
                        }
                        ChemCloud_OrderWithCoa co = new ChemCloud_OrderWithCoa()
                        {
                            CoaNo = item.coaNo,
                            ProductCode = item.code,
                            OrderId = OINFO.Id
                        };
                        ServiceHelper.Create<IChemCloud_OrderWithCoaService>().AddOrderWithCoa(co);
                    }
                    #endregion
                    num++;
                }
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ConfirmSendGoodFreight(string ids, string companyNames, string shipOrderNumbers, string coaid, int isreplacedeliver, string replacedeliveraddress,int freightId)
        {
            coaid = coaid.Substring(0, coaid.Length - 1);
            coaid = coaid + "]";
            List<Coaid> coalist = JsonConvert.DeserializeObject<List<Coaid>>(coaid);

            Result result = new Result();

            try
            {
                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from item in ids.Split(chrArray)
                    select long.Parse(item);
                string[] strArrays = companyNames.Split(new char[] { ',' });
                string[] strArrays1 = shipOrderNumbers.Split(new char[] { ',' });
                ServiceHelper.Create<IOrderService>().GetOrders(nums);
                int num = 0;
                foreach (long num1 in nums)
                {
                    ServiceHelper.Create<IOrderService>().SellerSendGoodFreight(num1, base.CurrentSellerManager.UserName, strArrays[num], strArrays1[num], isreplacedeliver, replacedeliveraddress, freightId);
                    #region 物流通关消息发送 xzg
                    IOrderService IOS = ServiceHelper.Create<IOrderService>();
                    OrderInfo OINFO = IOS.GetOrder(num1);
                    if (OINFO != null)
                    {
                        ServiceHelper.Create<ISiteMessagesService>().SendLogisticsClearanceMessage(OINFO.UserId, num1.ToString());
                    }

                    foreach (Coaid item in coalist)
                    {
                        if (item.coaNo == "请选择")
                        {
                            continue;
                        }
                        ChemCloud_OrderWithCoa co = new ChemCloud_OrderWithCoa()
                        {
                            CoaNo = item.coaNo,
                            ProductCode = item.code,
                            OrderId = OINFO.Id
                        };
                        ServiceHelper.Create<IChemCloud_OrderWithCoaService>().AddOrderWithCoa(co);
                    }
                    #endregion
                    num++;
                }
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }



        //private Cell CreateCell(int cellID, Row row)
        //{
        //    Cell cell = row.GetCell(cellID) ?? row.CreateCell(cellID);
        //    return cell;
        //}

        //private Row CreateRow(int rowID, HSSFSheet excelSheet)
        //{
        //    Row row = excelSheet.GetRow(rowID) ?? excelSheet.CreateRow(rowID);
        //    return row;
        //}

        public ActionResult Detail(long id, bool updatePrice = false)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(id);
            if (order == null || order.ShopId != base.CurrentSellerManager.ShopId)
            {
                throw new HimallException("订单已被删除，或者不属于该供应商！");
            }
            ViewBag.UpdatePrice = updatePrice;

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

            foreach (var r in order.OrderItemInfo)
            {
                /*订单查看时候，加载cas#产品图片*/
                r.Id = r.Id;
                r.CASNo = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).CASNo : "";
                r.MolecularFormula = ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId) != null ? ServiceHelper.Create<ChemCloud.IServices.IProductService>().GetProduct(r.ProductId).MolecularFormula : "";
                r.Pub_CID = ServiceHelper.Create<IProductService>().GetProduct(r.ProductId) == null ? 0 : (ServiceHelper.Create<IProductService>().GetProduct(r.ProductId).Pub_CID == null ? 0 : ServiceHelper.Create<IProductService>().GetProduct(r.ProductId).Pub_CID);
            }
            return View(order);
        }
        public ActionResult PrintInvoice(long Id)
        {
            OrderInfo orderInfo = ServiceHelper.Create<IOrderService>().GetOrder(Id);
            List<OrderItemInfo> orderItemInfo = ServiceHelper.Create<IOrderService>().GetOrderItemInfoDetailWithProductCode(Id);
            decimal total = 0;
            foreach (OrderItemInfo list in orderItemInfo)
            {
                list.SalePrice = list.SalePrice++;
                total = list.SalePrice;
            }
            ViewBag.email = ServiceHelper.Create<IMemberService>().GetMember(orderInfo.UserId).Email;
            ViewBag.invoice = GenerateInvoiceNumber();
            ViewBag.strDate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.total = total;
            return View(orderInfo);
        }
        [UnAuthorize]
        public FileResult DownloadOrderList(string ids)
        {
            base.HttpContext.Response.BufferOutput = true;
            DateTime now = DateTime.Now;
            string str = string.Concat("ordergoods_", now.ToString("yyyyMMddHHmmss"), ".xls");
            if (base.Request.ServerVariables["http_user_agent"].ToLower().IndexOf("firefox") == -1)
            {
                str = HttpUtility.UrlEncode(str, Encoding.UTF8);
            }
            HSSFWorkbook excel = writeToExcel(ids);
            MemoryStream memoryStream = new MemoryStream();
            excel.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.ms-excel", str);
        }

        public void DownloadProductList(string ids)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            Dictionary<long, OrderItemInfo> orderItems = ServiceHelper.Create<IOrderService>().GetOrderItems(nums);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
            stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
            stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            stringBuilder.AppendLine("<td>产品名称</td>");
            stringBuilder.AppendLine("<td>货号</td>");
            stringBuilder.AppendLine("<td>规格</td>");
            stringBuilder.AppendLine("<td>拣货数量</td>");
            stringBuilder.AppendLine("<td>现库存数</td>");
            stringBuilder.AppendLine("</tr>");
            long stock = 0;
            foreach (OrderItemInfo value in orderItems.Values)
            {
                SKUInfo sku = ServiceHelper.Create<IProductService>().GetSku(value.SkuId);
                if (sku != null)
                {
                    stock = sku.Stock;
                }
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine(string.Concat("<td>", value.ProductName, "</td>"));
                stringBuilder.AppendLine(string.Concat("<td style=\"vnd.ms-excel.numberformat:@\">", value.SKU, "</td>"));
                string[] color = new string[] { "<td>", value.Color, value.Size, value.Version, "</td>" };
                stringBuilder.AppendLine(string.Concat(color));
                stringBuilder.AppendLine(string.Concat("<td>", value.Quantity, "</td>"));
                long quantity = stock + value.Quantity;
                stringBuilder.AppendLine(string.Concat("<td>", quantity.ToString(), "</td>"));
                stringBuilder.AppendLine("</tr>");
            }
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</body></html>");
            base.Response.Clear();
            base.Response.Buffer = false;
            base.Response.Charset = "GB2312";
            HttpResponseBase response = base.Response;
            DateTime now = DateTime.Now;
            response.AppendHeader("Content-Disposition", string.Concat("attachment;filename=productgoods_", now.ToString("yyyyMMddHHmmss"), ".xls"));
            base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "application/ms-excel";
            base.Response.Write(stringBuilder.ToString());
            base.Response.End();
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult GetExpressData(string expressCompanyName, string shipOrderNumber)
        {

            if (string.IsNullOrWhiteSpace(expressCompanyName) || string.IsNullOrWhiteSpace(shipOrderNumber))
            {
                throw new HimallException("错误的订单信息");
            }

            OrderExpressQuery oe = ServiceHelper.Create<IOrderExpressQueryService>().GetOrderExpressById(shipOrderNumber);

            JsonResult result = Json(oe);
            return result;
            #region OLD
            //string kuaidi100Code = ServiceHelper.Create<IExpressService>().GetExpress(expressCompanyName).Kuaidi100Code;
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://www.kuaidi100.com/query?type={0}&postid={1}", kuaidi100Code, shipOrderNumber));
            //httpWebRequest.Timeout = 8000;
            //string end = "暂时没有此快递单号的信息";
            //try
            //{
            //    HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        Stream responseStream = response.GetResponseStream();
            //        StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
            //        end = streamReader.ReadToEnd();
            //        end = end.Replace("&amp;", "");
            //        end = end.Replace("&nbsp;", "");
            //        end = end.Replace("&", "");
            //    }
            //}
            //catch
            //{
            //}
            //return Json(end);
            #endregion
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
        [UnAuthorize]
        public JsonResult GetOrderPrint(string ids)
        {
            Result result = new Result();
            try
            {
                char[] chrArray = new char[] { ',' };
                IEnumerable<long> nums =
                    from item in ids.Split(chrArray)
                    select long.Parse(item);
                IEnumerable<OrderInfo> orders = ServiceHelper.Create<IOrderService>().GetOrders(nums);
                string siteName = base.CurrentSiteSetting.SiteName;
                StringBuilder stringBuilder = new StringBuilder();
                foreach (OrderInfo list in orders.ToList())
                {
                    object id = list.Id;
                    DateTime orderDate = list.OrderDate;
                    stringBuilder.AppendFormat("<h3 class=\"table-hd\"><strong>{0}发货单</strong><span>订单号：{1}（{2}）</span></h3>", siteName, id, orderDate.ToLongDateString());
                    stringBuilder.Append("<table class=\"table table-bordered\"><thead><tr><th>产品名称</th><th>规格</th><th>数量</th><th>单价</th><th>总价</th></tr></thead><tbody>");
                    foreach (OrderItemInfo orderItemInfo in list.OrderItemInfo.ToList())
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.AppendFormat("<td style=\"text-align:left\">{0}</td>", orderItemInfo.ProductName);
                        stringBuilder.AppendFormat("<td>{0} {1} {2}</td>", orderItemInfo.Color, orderItemInfo.Size, orderItemInfo.Version);
                        stringBuilder.AppendFormat("<td>{0}</td>", orderItemInfo.Quantity);
                        stringBuilder.AppendFormat("<td>￥{0}</td>", orderItemInfo.SalePrice);
                        stringBuilder.AppendFormat("<td>￥{0}</td>", orderItemInfo.RealTotalPrice);
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.AppendFormat("<tr><td style=\"text-align:right\" colspan=\"6\"><span>产品总价：￥{0} &nbsp; 运费：￥{1}</span> &nbsp; <b>实付金额：￥{2}</b></td></tr>", list.ProductTotalAmount, list.Freight, list.OrderTotalAmount);
                    stringBuilder.AppendLine("</tbody></table>");
                }
                result.success = true;
                result.msg = stringBuilder.ToString();
            }
            catch (Exception exception)
            {
                result.success = false;
            }
            return Json(result);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult GetRegion(long? key = null, int? level = -1)
        {
            int? nullable = level;
            if ((nullable.GetValueOrDefault() != -1 ? false : nullable.HasValue))
            {
                key = new long?(0);
            }
            if (!key.HasValue)
            {
                return Json(new object[0]);
            }
            IEnumerable<KeyValuePair<long, string>> region = ServiceHelper.Create<IRegionService>().GetRegion(key.Value);
            return Json(region);
        }

        [UnAuthorize]
        public JsonResult GetRegionIdPath(long regionId)
        {
            return Json(ServiceHelper.Create<IRegionService>().GetRegionIdPath(regionId));
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult List(DateTime? startDate, DateTime? endDate, long? orderId, int? orderStatus, string userName, int page, int rows, int? orderType, string casno)
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
            orderQuery.ShopId = new long?(base.CurrentSellerManager.ShopId);
            orderQuery.UserName = userName;

            orderQuery.CASNo = casno;

            orderQuery.OrderType = orderType;
            orderQuery.PageSize = rows;
            orderQuery.PageNo = page;
            PageModel<OrderInfo> orders = ServiceHelper.Create<IOrderService>().GetOrders<OrderInfo>(orderQuery, null);
            IEnumerable<OrderModel> array =
                from item in orders.Models.ToArray()
                select new OrderModel()
                {
                    OrderId = item.Id,
                    OrderStatus = item.OrderStatus.ToDescription(),
                    OrderDate = item.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ShopId = item.ShopId,
                    ShopName = item.ShopName,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    TotalPrice = item.OrderTotalAmount,
                    PaymentTypeName = item.PaymentTypeName,
                    IconSrc = GetIconSrc(item.Platform),
                    PlatForm = (int)item.Platform,
                    PlatformText = item.Platform.ToDescription(),
                    CoinType = item.CoinType,
                    CoinTypeName = item.CoinTypeName,
                    FinishDate = item.FinishDate == null ? "" : DateTime.Parse(item.FinishDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    ProductName = item.OrderItemInfo.FirstOrDefault() == null ? "" : item.OrderItemInfo.FirstOrDefault().ProductName,
                    CASNo = item.OrderItemInfo.FirstOrDefault() == null ? "" : item.OrderItemInfo.FirstOrDefault().CASNo,
                    Quantity = item.OrderItemInfo.FirstOrDefault() == null ? 0 : item.OrderItemInfo.FirstOrDefault().Quantity,
                    PackingUnit = item.OrderItemInfo.FirstOrDefault() == null ? "" : item.OrderItemInfo.FirstOrDefault().PackingUnit,
                    ShipOrderNumber = item.ShipOrderNumber,
                    ExpressCompanyName = item.ExpressCompanyName,

                    IsBehalfShip = item.IsBehalfShip,
                    BehalfShipType = item.BehalfShipType,
                    BehalfShipNumber = item.BehalfShipNumber
                };
            array = array.ToList();
            List<long> list = (
                from d in array
                select d.OrderId).ToList();
            if (list.Count > 0)
            {
                RefundQuery refundQuery = new RefundQuery()
                {
                    OrderId = new long?(list[0]),
                    MoreOrderId = list,
                    PageNo = 1,
                    PageSize = array.Count()
                };
                List<OrderRefundInfo> orderRefundInfos = (
                    from d in ServiceHelper.Create<IRefundService>().GetOrderRefunds(refundQuery).Models
                    where (int)d.RefundMode == 1 && (int)d.SellerAuditStatus != 4
                    select d).ToList();
                if (orderRefundInfos.Count > 0)
                {
                    foreach (OrderRefundInfo orderRefundInfo in orderRefundInfos)
                    {
                        OrderModel orderModel = array.FirstOrDefault((OrderModel d) => d.OrderId == orderRefundInfo.OrderId);
                        if (orderModel == null || !(orderModel.OrderStatus != OrderInfo.OrderOperateStatus.Close.ToDescription()) || orderRefundInfo.SellerAuditStatus == OrderRefundInfo.OrderRefundAuditStatus.UnAudit)
                        {
                            continue;
                        }
                        orderModel.RefundStats = (int)orderRefundInfo.SellerAuditStatus;
                    }
                }
            }
            DataGridModel<OrderModel> dataGridModel = new DataGridModel<OrderModel>()
            {
                rows = array,
                total = orders.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult Management()
        {
            return View();
        }

        public ActionResult Print(string orderIds)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in orderIds.Split(chrArray)
                select long.Parse(item);
            ViewBag.OrdersCount = nums.Count();
            ShopInfo shop = ServiceHelper.Create<IShopService>().GetShop(base.CurrentSellerManager.ShopId, false);
            ViewBag.Name = shop.SenderName;
            ViewBag.Address = shop.SenderAddress;
            ViewBag.Tel = shop.SenderPhone;
            if (!shop.SenderRegionId.HasValue)
            {
                ViewBag.RegionId = "";
                ViewBag.FullRegionPath = "";
            }
            else
            {
                string regionIdPath = ServiceHelper.Create<IRegionService>().GetRegionIdPath(shop.SenderRegionId.Value);
                dynamic viewBag = base.ViewBag;
                int? senderRegionId = shop.SenderRegionId;
                viewBag.RegionId = senderRegionId.Value;
                ViewBag.FullRegionPath = regionIdPath;
            }
            IEnumerable<IExpress> recentExpress = ServiceHelper.Create<IExpressService>().GetRecentExpress(base.CurrentSellerManager.ShopId, 2147483647);
            return View(recentExpress);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult Print(string orderIds, string expressName, string startNo, int regionId, string address, string senderName, string senderPhone)
        {
            IExpressService expressService = ServiceHelper.Create<IExpressService>();
            IExpress express = expressService.GetExpress(expressName);
            if (!express.CheckExpressCodeIsValid(startNo))
            {
                return Json(new { success = false, msg = "起始快递单号无效" });
            }
            ServiceHelper.Create<IShopService>().UpdateShopSenderInfo(base.CurrentSellerManager.ShopId, regionId, address, senderName, senderPhone);
            IEnumerable<int> elements =
                from item in express.Elements
                select item.PrintElementIndex;
            List<PrintModel> printModels = new List<PrintModel>();
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in orderIds.Split(chrArray)
                select long.Parse(item);
            foreach (long num in nums)
            {
                PrintModel printModel = new PrintModel()
                {
                    Width = express.Width,
                    Height = express.Height,
                    FontSize = 11
                };
                PrintModel printModel1 = printModel;
                IDictionary<int, string> printElementIndexAndOrderValue = expressService.GetPrintElementIndexAndOrderValue(base.CurrentSellerManager.ShopId, num, elements);
                printModel1.Elements = printElementIndexAndOrderValue.Select<KeyValuePair<int, string>, PrintModel.PrintElement>((KeyValuePair<int, string> item) =>
                {
                    ExpressPrintElement expressPrintElement = express.Elements.FirstOrDefault((ExpressPrintElement t) => t.PrintElementIndex == item.Key);
                    return new PrintModel.PrintElement()
                    {
                        X = expressPrintElement.LeftTopPoint.X,
                        Y = expressPrintElement.LeftTopPoint.Y,
                        Height = expressPrintElement.RightBottomPoint.Y - expressPrintElement.LeftTopPoint.Y,
                        Width = expressPrintElement.RightBottomPoint.X - expressPrintElement.LeftTopPoint.X,
                        Value = item.Value
                    };
                });
                printModels.Add(printModel1);
            }
            ServiceHelper.Create<IOrderService>().SetOrderExpressInfo(base.CurrentSellerManager.ShopId, expressName, startNo, nums);
            return Json(new { success = true, data = printModels });
        }

        public ActionResult SendGood(string ids)
        {
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            OrderController.SendGoodMode sendGoodMode = new OrderController.SendGoodMode();
            IOrderedEnumerable<OrderInfo> orderInfos = ServiceHelper.Create<IOrderService>().GetOrders(nums).Where((OrderInfo a) =>
            {
                //if (a.OrderStatus != OrderInfo.OrderOperateStatus.WaitDelivery)
                //{
                //    return false;
                //}
                return a.ShopId == base.CurrentSellerManager.ShopId;
            }).OrderByDescending<OrderInfo, DateTime>((OrderInfo a) => a.OrderDate);
            if (orderInfos == null)
            {
                throw new HimallException(string.Concat("没有找到相关的订单", ids));
            }
            List<OrderItemInfo> list = ServiceHelper.Create<IOrderService>().GetOrderItemInfoDetailWithProductCode(orderInfos.FirstOrDefault<OrderInfo>().Id);
            List<CoAShow> CoalistShow = new List<CoAShow>();
            foreach (OrderItemInfo item in list)
            {
                CoAShow cs = new CoAShow();
                cs.orderitem = item;

                cs.coalist = ServiceHelper.Create<ICOAListService>().GetUserCoaByCoaNo(base.CurrentUser.Id, item.CASNo);
                CoalistShow.Add(cs);
            }
            ViewBag.CoalistShow = CoalistShow;
            sendGoodMode.Orders = orderInfos;
            sendGoodMode.LogisticsCompanies = ServiceHelper.Create<IExpressService>().GetAllExpress();


            string PlatformCollectionAddress = "";
            SiteSettingsInfo siteSettings = ServiceHelper.Create<ISiteSettingService>().GetSiteSettings();
            if (siteSettings != null)
            {
                if (!string.IsNullOrWhiteSpace(siteSettings.PlatformCollectionAddress))
                {
                    PlatformCollectionAddress = siteSettings.PlatformCollectionAddress;
                }
            }
            ViewBag.PlatformCollectionAddress = PlatformCollectionAddress;



            IQueryable<FreightTemplateInfo> shopFreightTemplate = ServiceHelper.Create<IFreightTemplateService>().GetShopFreightTemplate(base.CurrentSellerManager.ShopId);
            List<SelectListItem> selectListItems3 = new List<SelectListItem>();

            SelectListItem selectListItem2 = new SelectListItem()
            {
                Selected = false,
                Text = "请选择运费模板...",
                Value = "0"
            };
            selectListItems3.Add(selectListItem2);

            foreach (FreightTemplateInfo freightTemplateInfo in shopFreightTemplate)
            {
                SelectListItem item = new SelectListItem()
                { 
                    Text = string.Concat(freightTemplateInfo.Name, "【", freightTemplateInfo.ValuationMethod.ToDescription(), "】"),
                    Value = freightTemplateInfo.Id.ToString()
                };
                selectListItems3.Add(item);
            }

            ViewBag.FreightTemplates = selectListItems3;

            return View(sendGoodMode);
        }

        //计算物流费用
        private decimal GetFreight(int freightTemplateId, List<OrderItemInfo> orderItemList, int cityId)
        {
            IFreightTemplateService freightTemplateService = ServiceHelper.Create<IFreightTemplateService>();
            decimal freight2=0;
            foreach (var orderItem in orderItemList)
            {
                FreightTemplateInfo freightTemplate = freightTemplateService.GetFreightTemplate(freightTemplateId);
                if (freightTemplate == null || freightTemplate.IsFree != FreightTemplateInfo.FreightTemplateType.SelfDefine)
                {
                    continue;
                }
                FreightAreaContentInfo freightAreaContentInfo = (
                    from item in freightTemplate.ChemCloud_FreightAreaContent
                    where item.AreaContent.Split(new char[] { ',' }).Contains<string>(cityId.ToString())
                    select item).FirstOrDefault() ?? freightTemplate.ChemCloud_FreightAreaContent.Where((FreightAreaContentInfo item) =>
                    {
                        byte? isDefault = item.IsDefault;
                        if (isDefault.GetValueOrDefault() != 1)
                        {
                            return false;
                        }
                        return isDefault.HasValue;
                    }).FirstOrDefault();
                if (freightTemplate.ValuationMethod == FreightTemplateInfo.ValuationMethodType.Weight)
                {
                    
                    int value = freightAreaContentInfo.FirstUnit.Value;
                    decimal value1 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value2 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(orderItem.Quantity, value, value1, value2, (decimal)accumulationUnitMoney.Value);
                }
                else if (freightTemplate.ValuationMethod != FreightTemplateInfo.ValuationMethodType.Bulk)
                {
                    int num4 = Convert.ToInt32(orderItem.Quantity);
                    decimal num5 = num4;
                    int value3 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value4 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int value5 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? nullable = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num5, value3, value4, value5, (decimal)nullable.Value);
                }
                else
                {
                    decimal num6 = Convert.ToInt32(orderItem.Quantity);
                    int value6 = freightAreaContentInfo.FirstUnit.Value;
                    decimal value7 = (decimal)freightAreaContentInfo.FirstUnitMonry.Value;
                    int num7 = freightAreaContentInfo.AccumulationUnit.Value;
                    float? accumulationUnitMoney1 = freightAreaContentInfo.AccumulationUnitMoney;
                    freight2 = freight2 + GetFreight2(num6, value6, value7, num7, (decimal)accumulationUnitMoney1.Value);
                }
            }

              return freight2;
        }

        private decimal GetFreight2(decimal count, int firstUnit, decimal firstUnitMonry, int accumulationUnit, decimal accumulationUnitMoney)
        {
            decimal num = new decimal(0);
            if (count > firstUnit)
            {
                decimal num1 = (count - firstUnit) / accumulationUnit;
                decimal num2 = Math.Truncate(num1);
                decimal num3 = num1 - num2;
                decimal num4 = num2 * accumulationUnitMoney;
                decimal num5 = new decimal(0);
                if (num3 > new decimal(0))
                {
                    num5 = new decimal(1) * accumulationUnitMoney;
                }
                num = (firstUnitMonry + num4) + num5;
            }
            else
            {
                num = firstUnitMonry;
            }
            return num;
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
        [HttpPost]
        [UnAuthorize]
        public JsonResult UpdateAddress(long orderId, string shipTo, string cellPhone, int topRegionId, int regionId, string address)
        {
            Result result = new Result();
            try
            {
                string regionFullName = ServiceHelper.Create<IRegionService>().GetRegionFullName(regionId, " ");
                ServiceHelper.Create<IOrderService>().SellerUpdateAddress(orderId, base.CurrentSellerManager.UserName, shipTo, cellPhone, topRegionId, regionId, regionFullName, address);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult UpdateItemDiscountAmount(long orderItemId, decimal discountAmount)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IOrderService>().SellerUpdateItemDiscountAmount(orderItemId, discountAmount, base.CurrentSellerManager.UserName);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateOrderFrieght(long orderId, decimal frieght)
        {
            ServiceHelper.Create<IOrderService>().SellerUpdateOrderFreight(orderId, frieght);
            return Json(new { success = true });
        }

        private HSSFWorkbook writeToExcel(string ids)
        {
            long stock = 0;
            char[] chrArray = new char[] { ',' };
            IEnumerable<long> nums =
                from item in ids.Split(chrArray)
                select long.Parse(item);
            IEnumerable<OrderInfo> orders = ServiceHelper.Create<IOrderService>().GetOrders(nums);
            HSSFWorkbook hSSFWorkbook = new HSSFWorkbook();
            HSSFSheet hSSFSheet = (HSSFSheet)hSSFWorkbook.CreateSheet("sheet1");
            //Row row = hSSFSheet.CreateRow(0);
            //string[] strArrays = new string[] { "订单单号", "产品名称", "货号", "规格", "拣货数量", "现库存数", "备注" };
            //string[] strArrays1 = strArrays;
            //for (int i = 0; i < strArrays1.Length; i++)
            //{
            //    CreateCell(i, row).SetCellValue(strArrays1[i]);
            //}
            //foreach (OrderInfo list in orders.ToList())
            //{
            //    foreach (OrderItemInfo orderItemInfo in list.OrderItemInfo.ToList())
            //    {
            //        SKUInfo sku = ServiceHelper.Create<IProductService>().GetSku(orderItemInfo.SkuId);
            //        if (sku != null)
            //        {
            //            stock = sku.Stock;
            //        }
            //        Row row1 = CreateRow(hSSFSheet.PhysicalNumberOfRows, hSSFSheet);
            //        Cell cell = CreateCell(0, row1);
            //        CellStyle format = hSSFWorkbook.CreateCellStyle();
            //        format.DataFormat = hSSFWorkbook.CreateDataFormat().GetFormat("####################");
            //        cell.CellStyle = format;
            //        cell.SetCellValue(list.Id);
            //        cell = CreateCell(1, row1);
            //        cell.SetCellValue(orderItemInfo.ProductName);
            //        cell = CreateCell(2, row1);
            //        cell.SetCellValue(orderItemInfo.SKU);
            //        cell = CreateCell(3, row1);
            //        cell.SetCellValue(string.Concat(orderItemInfo.Color, orderItemInfo.Size, orderItemInfo.Version));
            //        cell = CreateCell(4, row1);
            //        cell.SetCellValue(orderItemInfo.Quantity);
            //        cell = CreateCell(5, row1);
            //        cell.SetCellValue((stock + orderItemInfo.Quantity).ToString());
            //        cell = CreateCell(6, row1);
            //        cell.SetCellValue(list.UserRemark);
            //    }
            //}
            return hSSFWorkbook;
        }

        public class SendGoodMode
        {
            public IEnumerable<IExpress> LogisticsCompanies
            {
                get;
                set;
            }

            public IEnumerable<OrderInfo> Orders
            {
                get;
                set;
            }

            

            public SendGoodMode()
            {
            }
        }

        [HttpPost]
        public JsonResult GetOrderInfo(string orderid)
        {
            Result result = new Result();
            try
            {
                OrderInfo model = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
                if (model != null && model.OrderItemInfo.FirstOrDefault() != null)
                {
                    result.msg = model.OrderItemInfo.FirstOrDefault().ProductId + "^" + model.OrderItemInfo.FirstOrDefault().ProductName;
                }
                else { result.msg = "0^0"; }
                result.success = true;

            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }


        [HttpPost]
        public JsonResult AddCOA(string json)
        {
            Result result = new Result();
            try
            {
                ChemCloud_COA model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChemCloud_COA>(json); //序列化json
                model.Supplier = base.CurrentSellerManager.UserName;
                model.DATEOFRELEASE = DateTime.Now;
                ServiceHelper.Create<ICOAService>().AddCOA(model);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
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
                ViewBag.CodePath = "/Storage/Code128/" + path;
            }
            else
            {
                ViewBag.CodePath = "";
            }
            return View(order);
        }


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

            return View(order);
        }


        [HttpPost]
        [UnAuthorize]
        public JsonResult Updateorderprice(string strjson)
        {
            Result result = new Result();
            try
            {
                orderprice model = Newtonsoft.Json.JsonConvert.DeserializeObject<orderprice>(strjson);
                if (model != null)
                {
                    long orderid = long.Parse(model.orderid);
                    decimal productamount = model.totalproductamount;

                    ServiceHelper.Create<IOrderService>().updateorderproductamount(orderid, productamount);
                    /*改订单产品总价*/
                    foreach (var item in model._orderpriceitem)
                    {
                        /*改订详细产品价格*/
                        long orderitemid = long.Parse(item.orderitemid);
                        decimal price = item.price;
                        ServiceHelper.Create<IOrderService>().updateorderitemprice(orderitemid, price);
                    }

                }
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }
    }

    public class orderprice
    {
        public string orderid { get; set; }
        public decimal totalproductamount { get; set; }
        public List<orderpriceitem> _orderpriceitem { get; set; }
    }

    public class orderpriceitem
    {
        public string orderitemid { get; set; }
        public decimal price { get; set; }
    }

    public class Coaid
    {
        public string code { get; set; }
        public string coaNo { get; set; }
    }

    public class CoAShow
    {
        public OrderItemInfo orderitem { get; set; }

        public List<COAList> coalist { get; set; }
    }

}