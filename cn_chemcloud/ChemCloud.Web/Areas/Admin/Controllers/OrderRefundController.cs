using ChemCloud.Core;
using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class OrderRefundController : BaseAdminController
    {
        public OrderRefundController()
        {
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult ConfirmRefund(long refundId, string managerRemark)
        {
            Result result = new Result();
            IRefundService refundService = ServiceHelper.Create<IRefundService>();
            refundService.ConfirmRefund(refundId, managerRemark, base.CurrentManager.UserName);
            result.success = true;
            return Json(result);
        }

        public static string HTMLEncode(string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return string.Empty;
            }
            string str = txt.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\"", "&quot;");
            return str.Replace("'", "&#39;").Replace("\n", "<br>");
        }

        [HttpPost]
        [UnAuthorize]
        [ValidateInput(false)]
        public JsonResult List(DateTime? startDate, DateTime? endDate, long? orderId, int? auditStatus, string shopName, string ProductName, string userName, int page, int rows, int showtype = 0)
        {
            OrderRefundInfo.OrderRefundAuditStatus? nullable;
            RefundQuery refundQuery = new RefundQuery()
            {
                StartDate = startDate,
                EndDate = endDate,
                OrderId = orderId,
                ProductName = ProductName
            };
            RefundQuery refundQuery1 = refundQuery;
            int? nullable1 = auditStatus;
            if (nullable1.HasValue)
            {
                nullable = new OrderRefundInfo.OrderRefundAuditStatus?((OrderRefundInfo.OrderRefundAuditStatus)nullable1.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            refundQuery1.AuditStatus = nullable;
            refundQuery.ShopName = shopName;
            refundQuery.UserName = userName;
            refundQuery.PageSize = rows;
            refundQuery.PageNo = page;
            refundQuery.ShowRefundType = new int?(showtype);
            RefundQuery nullable2 = refundQuery;
            if (auditStatus.HasValue && auditStatus.Value == 5)
            {
                nullable2.ConfirmStatus = new OrderRefundInfo.OrderRefundConfirmStatus?(OrderRefundInfo.OrderRefundConfirmStatus.UnConfirm);
            }
            PageModel<OrderRefundInfo> orderRefunds = ServiceHelper.Create<IRefundService>().GetOrderRefunds(nullable2);
            IEnumerable<OrderRefundModel> orderRefundModels = ((IEnumerable<OrderRefundInfo>)orderRefunds.Models.ToArray()).Select<OrderRefundInfo, OrderRefundModel>((OrderRefundInfo item) =>
            {
                string str = string.Concat((string.IsNullOrWhiteSpace(item.OrderItemInfo.Color) ? "" : string.Concat(item.OrderItemInfo.Color, "，")), (string.IsNullOrWhiteSpace(item.OrderItemInfo.Size) ? "" : string.Concat(item.OrderItemInfo.Size, "，")), (string.IsNullOrWhiteSpace(item.OrderItemInfo.Version) ? "" : string.Concat(item.OrderItemInfo.Version, "，"))).TrimEnd(new char[] { '，' });
                if (!string.IsNullOrWhiteSpace(str))
                {
                    str = string.Concat("  【", str, " 】");
                }
                return new OrderRefundModel()
                {
                    RefundId = item.Id,
                    OrderId = item.OrderId,
                    AuditStatus = (item.SellerAuditStatus == OrderRefundInfo.OrderRefundAuditStatus.Audited ? item.ManagerConfirmStatus.ToDescription() : item.SellerAuditStatus.ToDescription()),
                    ProductId = item.OrderItemInfo.ProductId,
                    ThumbnailsUrl = item.OrderItemInfo.ThumbnailsUrl,
                    ConfirmStatus = item.ManagerConfirmStatus.ToDescription(),
                    ApplyDate = item.ApplyDate.ToShortDateString(),
                    ShopId = item.ShopId,
                    ShopName = item.ShopName.Replace("'", "‘").Replace("\"", "”"),
                    UserId = item.UserId,
                    CompanyName = item.CompanyName,
                    UserName = item.Applicant,
                    Amount = item.Amount.ToString("F2"),
                    ReturnQuantity = item.OrderItemInfo.ReturnQuantity,
                    ProductName = string.Concat(item.OrderItemInfo.ProductName, str),
                    Reason = (string.IsNullOrEmpty(item.Reason) ? string.Empty : OrderRefundController.HTMLEncode(item.Reason.Replace("'", "‘").Replace("\"", "”"))),
                    RefundAccount = (string.IsNullOrEmpty(item.RefundAccount) ? string.Empty : OrderRefundController.HTMLEncode(item.RefundAccount.Replace("'", "‘").Replace("\"", "”"))),
                    ContactPerson = (string.IsNullOrEmpty(item.ContactPerson) ? string.Empty : OrderRefundController.HTMLEncode(item.ContactPerson.Replace("'", "‘").Replace("\"", "”"))),
                    ContactCellPhone = OrderRefundController.HTMLEncode(item.ContactCellPhone),
                    PayeeAccount = (string.IsNullOrEmpty(item.PayeeAccount) ? string.Empty : OrderRefundController.HTMLEncode(item.PayeeAccount.Replace("'", "‘").Replace("\"", "”"))),
                    Payee = (string.IsNullOrEmpty(item.Payee) ? string.Empty : OrderRefundController.HTMLEncode(item.Payee)),
                    RefundMode = (int)item.RefundMode,
                    SellerRemark = (string.IsNullOrEmpty(item.SellerRemark) ? string.Empty : OrderRefundController.HTMLEncode(item.SellerRemark.Replace("'", "‘").Replace("\"", "”"))),
                    ManagerRemark = (string.IsNullOrEmpty(item.ManagerRemark) ? string.Empty : OrderRefundController.HTMLEncode(item.ManagerRemark.Replace("'", "‘").Replace("\"", "”"))),
                    RefundStatus = (item.SellerAuditStatus == OrderRefundInfo.OrderRefundAuditStatus.Audited ? item.ManagerConfirmStatus.ToDescription() : item.SellerAuditStatus.ToDescription()),
                    RefundPayType = (!item.RefundPayType.HasValue ? "线下处理" : ((Enum)(object)item.RefundPayType).ToDescription())
                };
            });
            DataGridModel<OrderRefundModel> dataGridModel = new DataGridModel<OrderRefundModel>()
            {
                rows = orderRefundModels,
                total = orderRefunds.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult Management(int showtype = 0)
        {
            ViewBag.ShowType = showtype;
            return View();
        }

        public ActionResult TH() { return View(); }

        public ActionResult TH1() { return View(); }


        /// <summary>
        /// 退款列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TK() { return View(); }

        /// <summary>
        ///  平台退款详细页面
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult TKDetail(long orderNo)
        {
            try
            {
                TK tk = ServiceHelper.Create<ITKService>().getTK(orderNo);

                if (tk != null)
                {
                    ViewBag.type = tk.TKType;
                    ViewBag.ReasonType = tk.ReasonType;
                    ViewBag.TKAmont = tk.TKAmont;
                    ViewBag.TKInstruction = tk.TKInstruction;
                }
                else
                {
                    ViewBag.type = 1;
                    ViewBag.ReasonType = 0;
                    ViewBag.TKAmont = 0;
                    ViewBag.TKInstruction = "";
                }

                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(orderNo));
                List<TKMessageModel> tkmms = new List<TKMessageModel>();
                List<TKMessage> tks = ServiceHelper.Create<ITKService>().getTKMessage(tk.Id);
                if (tks != null)
                {
                    foreach (TKMessage item in tks)
                    {
                        TKMessageModel tkmm = new TKMessageModel()
                        {
                            MessageAttitude = item.MessageAttitude,
                            ReturnName = item.ReturnName,
                            MessageDate = item.MessageDate,
                            MessageContent = item.MessageContent,
                            UserId = item.UserId,
                            TKId = item.TKId,
                            Id = item.Id,
                            tkis = ServiceHelper.Create<ITKService>().getTKImage(item.Id)
                        };
                        tkmms.Add(tkmm);
                        ViewBag.MessageContent += "" + item.MessageContent;
                    }
                    ViewBag.tkmms = tkmms;
                }
                if (order != null)
                {
                    ViewBag.OrderNo = orderNo;
                    ViewBag.UserName = order.UserName;
                    ViewBag.yunfei = order.Freight;
                    ViewBag.total = order.ProductTotalAmount;
                    ViewBag.max = order.Freight + order.ProductTotalAmount;
                    ViewBag.ShopName = order.ShopName;
                }
                else
                {
                    ViewBag.OrderNo = "";
                    ViewBag.UserName = "";
                    ViewBag.yunfei = 0;
                    ViewBag.total = 0;
                    ViewBag.max = 0;
                    ViewBag.ShopName = "";
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        /// <summary>
        /// 审核退款通过
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckTK(long OrderNo)
        {
            Result res = new Result();
            try
            {
                /*1、更改退款单的状态*/
                ServiceHelper.Create<ITKService>().UpdateTK(OrderNo, 5);

                /*2、更改订单的状态 单状态为已退款*/
                ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 11);

                /*3、转账*/
                TK tk = ServiceHelper.Create<ITKService>().getTK(OrderNo);
                long buyid = tk.UserId;
                long sellid = ServiceHelper.Create<IShopService>().GetMemberInfoByShopid(tk.SellerUserId) == null ? 0 : ServiceHelper.Create<IShopService>().GetMemberInfoByShopid(tk.SellerUserId).Id;
                decimal tkamount = tk.TKAmont;

                /* 获取转账方的用户id和用户类型 供应商2*/
                Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(sellid, 2, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
                fwinfo.Wallet_UserLeftMoney = fwinfo.Wallet_UserLeftMoney - tkamount;//获取当前用的可用金额

                /* 获取转账接受方的用户id和用户类型 采购商3*/
                Finance_Wallet fwinfoto = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(buyid, 3, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
                fwinfoto.Wallet_UserLeftMoney = fwinfoto.Wallet_UserLeftMoney + tkamount;//获取当前用的可用金额

                /*添加财务转账信息*/

                Finance_Transfer ftinfo = new Finance_Transfer();
                ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                ftinfo.Trans_Number = _orderBO.GenerateOrderNumber();/*创建转账单号*/
                ftinfo.Trans_UserId = sellid;
                ftinfo.Trans_UserType = 2;
                ftinfo.Trans_Money = tkamount;
                ftinfo.Trans_SXMoney = 0;
                ftinfo.Trans_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
                ftinfo.Trans_Time = DateTime.Now;
                ftinfo.Trans_Address = ChemCloud.Core.Common.GetIpAddress();
                ftinfo.Trans_ToUserId = buyid;
                ftinfo.Trans_ToUserType = 3;
                ftinfo.Trans_Status = 1;

                //添加财务退款信息
                Finance_Refund frinfo = new Finance_Refund();
                frinfo.Refund_Number = _orderBO.GenerateOrderNumber();//创建退款单号
                frinfo.Refund_OrderNum = OrderNo;
                frinfo.Refund_UserId = sellid;
                frinfo.Refund_UserType = ServiceHelper.Create<IMemberService>().GetMember(sellid) == null ? 2 : ServiceHelper.Create<IMemberService>().GetMember(sellid).UserType;
                frinfo.Refund_UserName = ServiceHelper.Create<IMemberService>().GetMember(sellid).UserName == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(sellid).UserName;
                frinfo.Refund_Money = tkamount;
                frinfo.Refund_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
                frinfo.Refund_SXMoney = 0;
                frinfo.Refund_ISChujing = 0;
                frinfo.Refund_Address = ChemCloud.Core.Common.GetIpAddress();
                frinfo.Refund_Time = tk.TKDate;
                frinfo.Refund_Status = 1;
                frinfo.Refund_ToUserId = buyid;
                frinfo.Refund_ToUserType = 3;
                frinfo.Refund_ToUserName = ServiceHelper.Create<IMemberService>().GetMember(buyid).UserName == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(buyid).UserName;

                ServiceHelper.Create<IFinance_TransferService>().AddFinance_Transfer(ftinfo);
                ServiceHelper.Create<IFinance_RefundService>().AddFinance_Refund(frinfo);
                ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo);
                ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfoto);
                res.success = true;
            }
            catch (Exception)
            {
                res.success = false;
            }
            return Json(res);
        }


        public ActionResult TK1() { return View(); }
        /// <summary>
        /// 所有的退货列表查询
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="OrderNum">订单号</param>
        /// <param name="ProductName">产品名称</param>
        /// <param name="username">供应商</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListTH(int page, int rows, string starttime, string endtime, string OrderNum, string ProductName, string username, string status)
        {

            THQuery fpq = new THQuery();

            if (status == "1")
            {
                fpq.Status = 6;
            }
            else
            {
                fpq.Status = 0;
            }
            long inti;
            if (long.TryParse(OrderNum, out inti))
            {
                fpq.orderNum = long.Parse(OrderNum);
            }
            fpq.productName = ProductName;
            fpq.muserid = 0;
            fpq.username = username;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.PageSize = rows;
            fpq.PageNo = page;


            PageModel<TH> fp = ServiceHelper.Create<ITHService>().GetTHListInfo(fpq);
            IEnumerable<TH> models =
            from info in fp.Models.ToArray()
            select new TH()
            {
                Id = info.Id,
                TH_Number = info.TH_Number,
                TH_OrderNum = info.TH_OrderNum,
                TH_Time = info.TH_Time,
                TH_UserId = info.TH_UserId,
                TH_UserName = info.TH_UserName,
                TH_UserType = info.TH_UserType,
                TH_ProductName = info.TH_ProductName,
                TH_ProductCount = info.TH_ProductCount,
                TH_ProductMoney = info.TH_ProductMoney,
                TH_ProductMoneyReal = info.TH_ProductMoneyReal,
                TH_ProductMoneyType = info.TH_ProductMoneyType,
                TH_ToUserId = info.TH_ToUserId,
                TH_ToUserName = info.TH_ToUserName,
                TH_ToUserType = info.TH_ToUserType,
                TH_Status = info.TH_Status
            };
            DataGridModel<TH> dataGridModel = new DataGridModel<TH>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }


        /// <summary>
        /// 所有的退货列表查询
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="OrderNum">订单号</param>
        /// <param name="ProductName">产品名称</param>
        /// <param name="username">供应商</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListTH1(int page, int rows, string starttime, string endtime, string OrderNum, string ProductName, string username)
        {
            THQuery fpq = new THQuery();
            long inti;
            if (long.TryParse(OrderNum, out inti))
            {
                fpq.orderNum = long.Parse(OrderNum);
            }
            fpq.productName = ProductName;
            fpq.muserid = 0;
            fpq.username = username;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<TH> fp = ServiceHelper.Create<ITHService>().GetTHListInfo1(fpq);
            IEnumerable<TH> models =
            from info in fp.Models.ToArray()
            select new TH()
            {
                Id = info.Id,
                TH_Number = info.TH_Number,
                TH_OrderNum = info.TH_OrderNum,
                TH_Time = info.TH_Time,
                TH_UserId = info.TH_UserId,
                TH_UserName = info.TH_UserName,
                TH_UserType = info.TH_UserType,
                TH_ProductName = info.TH_ProductName,
                TH_ProductCount = info.TH_ProductCount,
                TH_ProductMoney = info.TH_ProductMoney,
                TH_ProductMoneyReal = info.TH_ProductMoneyReal,
                TH_ProductMoneyType = info.TH_ProductMoneyType,
                TH_ToUserId = info.TH_ToUserId,
                TH_ToUserName = info.TH_ToUserName,
                TH_ToUserType = info.TH_ToUserType,
                TH_Status = info.TH_Status
            };
            DataGridModel<TH> dataGridModel = new DataGridModel<TH>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }
        /// <summary>
        /// 供应商退款列表查询
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="OrderNum">订单号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListTK(int page, int rows, string status, string OrderNum)
        {
            string OrderNo = "0";
            if (!string.IsNullOrEmpty(OrderNum))
            {
                OrderNo = OrderNum;
            }
            TKQuery tq = new TKQuery()
            {
                TKType = int.Parse(status),
                PageNo = page,
                PageSize = rows,
                OrderNo = Convert.ToInt64(OrderNo)
            };
            PageModel<TK> tk = ServiceHelper.Create<ITKService>().getTkList(tq, 0, 0);
            var array =
                from item in tk.Models.ToArray()
                select new
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    TKAmont = item.TKAmont,
                    TKDate = item.TKDate,
                    EndDate = item.EndDate,
                    TKResion = item.TKResion,
                    TKType = item.TKType,
                    BuyerName = ServiceHelper.Create<IMemberService>().GetMember(item.UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.UserId).UserName,
                    SellerName = ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(item.SellerUserId) == null ? "" : ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(item.SellerUserId).UserName,
                };
            return Json(new { rows = array, total = tk.Total });
        }
        /// <summary>
        /// 供应商退款列表查询
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="rows">行数</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="OrderNum">订单号</param>
        /// <param name="username">采购商名称</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListTK1(int page, int rows, string starttime, string endtime, string OrderNum, string username)
        {
            Finance_RefundQuery fpq = new Finance_RefundQuery();
            long inti;
            if (long.TryParse(OrderNum, out inti))
            {
                fpq.orderNum = long.Parse(OrderNum);
            }
            fpq.musername = username;
            fpq.userid = 0;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Refund> fp = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundListInfo1(fpq);
            IEnumerable<Finance_Refund> models =
            from item in fp.Models.ToArray()
            select new Finance_Refund()
            {
                Id = item.Id,
                Refund_Number = item.Refund_Number,
                Refund_OrderNum = item.Refund_OrderNum,
                Refund_UserId = item.Refund_UserId,
                Refund_UserType = item.Refund_UserType,
                Refund_UserName = item.Refund_UserName,
                Refund_Money = item.Refund_Money,
                Refund_MoneyType = item.Refund_MoneyType,
                Refund_SXMoney = item.Refund_SXMoney,
                Refund_ISChujing = item.Refund_ISChujing,
                Refund_Address = item.Refund_Address,
                Refund_Time = item.Refund_Time,
                Refund_Status = item.Refund_Status,
                Refund_ToUserId = item.Refund_ToUserId,
                Refund_ToUserType = item.Refund_ToUserType,
                Refund_ToUserName = item.Refund_ToUserName
            };
            DataGridModel<Finance_Refund> dataGridModel = new DataGridModel<Finance_Refund>()
            {
                rows = models,
                total = fp.Total
            };
            return Json(dataGridModel);
        }


        /// <summary>
        ///  平台退货详细页面
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult THDetail(long orderNo)
        {
            try
            {
                TH th = ServiceHelper.Create<ITHService>().GetTHByOrderNum(orderNo);
                if (th == null)
                {
                    decimal newmoney = 0;
                    OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(orderNo);
                    OrderItemInfo oiiinfo = ServiceHelper.Create<IOrderService>().GetOrderItemInfo(orderNo);
                    if (oiiinfo != null || oinfo != null)
                    {
                        newmoney = oinfo.OrderTotalAmount - (oinfo.Freight + oinfo.Insurancefee + oinfo.Transactionfee + oinfo.Counterfee);
                    }
                    ViewBag.TH_Reason = "";
                    ViewBag.TH_ProductMoney = newmoney;
                    ViewBag.TH_Status = 0;

                    th.TH_WLDH = "";
                    th.TH_WLGS = "";
                }
                else
                {
                    ViewBag.TH_Reason = th.TH_Reason;
                    ViewBag.TH_ProductMoney = th.TH_ProductMoney;
                    ViewBag.TH_Status = th.TH_Status;

                    List<THMessageModel> tkmms = new List<THMessageModel>();
                    List<THMessageInfo> tks = ServiceHelper.Create<ITHService>().getTHMessage(th.Id);
                    foreach (THMessageInfo item in tks)
                    {
                        THMessageModel tkmm = new THMessageModel()
                        {
                            MessageAttitude = item.MessageAttitude,
                            ReturnName = item.ReturnName,
                            MessageDate = item.MessageDate,
                            MessageContent = item.MessageContent,
                            UserId = item.UserId,
                            THId = item.THId,
                            Id = item.Id,
                            tkis = ServiceHelper.Create<ITHService>().getTHImage(item.Id)
                        };
                        tkmms.Add(tkmm);
                    }
                    ViewBag.tkmms = tkmms;
                }

                /*订单信息*/
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(orderNo));
                if (order != null)
                {
                    ViewBag.OrderNo = order.Id;
                    ViewBag.ShopName = order.ShopName;
                    ViewBag.yunfei = order.Freight;
                    ViewBag.total = order.ProductTotalAmount;
                    ViewBag.max = order.Freight + order.ProductTotalAmount;
                    ViewBag.ShopId = order.ShopId;
                    ViewBag.UserName = order.UserName;
                }
                else
                {
                    ViewBag.OrderNo = "";
                    ViewBag.ShopName = "";
                    ViewBag.yunfei = "";
                    ViewBag.total = "";
                    ViewBag.max = "";
                    ViewBag.ShopId = "";
                }

                return View(th);
            }
            catch (Exception)
            {
                return View();
            }
        }

        /*审核退货单*/
        public JsonResult CheckTH(long id)
        {
            Result res = new Result();

            try
            {
                /*1、转账*/
                TH tk = ServiceHelper.Create<ITHService>().GetTHByOrderNum(id);

                if (tk != null)
                {

                    long buyid = tk.TH_UserId;
                    long sellid = tk.TH_ToUserId;
                    decimal tkamount = tk.TH_ProductMoney;

                    /* 获取转账方的用户id和用户类型 供应商2*/
                    Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(sellid, 2, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
                    fwinfo.Wallet_UserLeftMoney = fwinfo.Wallet_UserLeftMoney - tkamount;//获取当前用的可用金额

                    /* 获取转账接受方的用户id和用户类型 采购商3*/
                    Finance_Wallet fwinfoto = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(buyid, 3, int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString()));
                    fwinfoto.Wallet_UserLeftMoney = fwinfoto.Wallet_UserLeftMoney + tkamount;//获取当前用的可用金额

                    /*添加财务转账信息*/
                    Finance_Transfer ftinfo = new Finance_Transfer();
                    ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                    ftinfo.Trans_Number = _orderBO.GenerateOrderNumber();/*创建转账单号*/
                    ftinfo.Trans_UserId = sellid;
                    ftinfo.Trans_UserType = 2;
                    ftinfo.Trans_Money = tkamount;
                    ftinfo.Trans_SXMoney = 0;
                    ftinfo.Trans_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
                    ftinfo.Trans_Time = DateTime.Now;
                    ftinfo.Trans_Address = ChemCloud.Core.Common.GetIpAddress();
                    ftinfo.Trans_ToUserId = buyid;
                    ftinfo.Trans_ToUserType = 3;
                    ftinfo.Trans_Status = 1;

                    ServiceHelper.Create<IFinance_TransferService>().AddFinance_Transfer(ftinfo);
                    ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo);
                    ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfoto);


                    //添加财务退款信息
                    Finance_Refund frinfo = new Finance_Refund();
                    frinfo.Refund_Number = _orderBO.GenerateOrderNumber();//创建退款单号
                    frinfo.Refund_OrderNum = id;
                    frinfo.Refund_UserId = sellid;
                    frinfo.Refund_UserType = 2;
                    frinfo.Refund_UserName = ServiceHelper.Create<IMemberService>().GetMember(sellid).UserName == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(sellid).UserName;
                    frinfo.Refund_Money = tkamount;
                    frinfo.Refund_MoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"].ToString());
                    frinfo.Refund_SXMoney = 0;
                    frinfo.Refund_ISChujing = 0;
                    frinfo.Refund_Address = ChemCloud.Core.Common.GetIpAddress();
                    frinfo.Refund_Time = tk.TH_Time;
                    frinfo.Refund_Status = 1;
                    frinfo.Refund_ToUserId = buyid;
                    frinfo.Refund_ToUserType = 3;
                    frinfo.Refund_ToUserName = ServiceHelper.Create<IMemberService>().GetMember(buyid).UserName == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(buyid).UserName;

                    ServiceHelper.Create<IFinance_RefundService>().AddFinance_Refund(frinfo);



                    /*2、更改退货单的状态 为已完成*/
                    ServiceHelper.Create<ITHService>().UpdateTHStatus(id, 6);

                    /*3更改订单的状态 单状态为已退货*/
                    ServiceHelper.Create<IOrderService>().UpdateOrderStatu(id, 9);

                    res.success = true;
                }
                else
                {
                    res.success = false;
                }
            }
            catch (Exception)
            {
                res.success = false;
            }

            return Json(res);
        }
    }
}