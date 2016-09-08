using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class OrderRefundController : BaseSellerController
    {
        public OrderRefundController()
        {
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult ConfirmRefundGood(long refundId)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IRefundService>().SellerConfirmRefundGood(refundId, base.CurrentSellerManager.UserName);
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
        public JsonResult DealRefund(long refundId, int auditStatus, string sellerRemark)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IRefundService>().SellerDealRefund(refundId, (OrderRefundInfo.OrderRefundAuditStatus)auditStatus, sellerRemark, base.CurrentSellerManager.UserName);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
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
        public JsonResult List(DateTime? startDate, DateTime? endDate, long? orderId, int? auditStatus, string userName, string ProductName, int page, int rows, int showtype = 0)
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
            refundQuery.ShopId = new long?(base.CurrentSellerManager.ShopId);
            refundQuery.UserName = userName;
            refundQuery.PageSize = rows;
            refundQuery.PageNo = page;
            refundQuery.ShowRefundType = new int?(showtype);
            PageModel<OrderRefundInfo> orderRefunds = ServiceHelper.Create<IRefundService>().GetOrderRefunds(refundQuery);
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
                    AuditStatus = item.SellerAuditStatus.ToDescription(),
                    ConfirmStatus = item.ManagerConfirmStatus.ToDescription(),
                    ApplyDate = item.ApplyDate.ToShortDateString(),
                    ShopId = item.ShopId,
                    ShopName = item.ShopName.Replace("'", "‘").Replace("\"", "”"),
                    UserId = item.UserId,
                    UserName = item.Applicant,
                    ContactPerson = OrderRefundController.HTMLEncode(item.ContactPerson),
                    ContactCellPhone = OrderRefundController.HTMLEncode(item.ContactCellPhone),
                    RefundAccount = (string.IsNullOrEmpty(item.RefundAccount) ? string.Empty : OrderRefundController.HTMLEncode(item.RefundAccount.Replace("'", "‘").Replace("\"", "”"))),
                    Amount = item.Amount.ToString("F2"),
                    ReturnQuantity = (item.RefundMode == OrderRefundInfo.OrderRefundMode.OrderRefund ? 0 : item.OrderItemInfo.ReturnQuantity),
                    Quantity = item.OrderItemInfo.Quantity,
                    SalePrice = item.EnabledRefundAmount.ToString("F2"),
                    ProductName = (item.RefundMode == OrderRefundInfo.OrderRefundMode.OrderRefund ? "订单所有产品" : string.Concat(item.OrderItemInfo.ProductName, str)),
                    Reason = (string.IsNullOrEmpty(item.Reason) ? string.Empty : OrderRefundController.HTMLEncode(item.Reason.Replace("'", "‘").Replace("\"", "”"))),
                    ExpressCompanyName = OrderRefundController.HTMLEncode(item.ExpressCompanyName),
                    ShipOrderNumber = item.ShipOrderNumber,
                    Payee = (string.IsNullOrEmpty(item.Payee) ? string.Empty : OrderRefundController.HTMLEncode(item.Payee)),
                    PayeeAccount = (string.IsNullOrEmpty(item.PayeeAccount) ? string.Empty : OrderRefundController.HTMLEncode(item.PayeeAccount.Replace("'", "‘").Replace("\"", "”"))),
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

        public ActionResult TH()
        {
            return View();
        }
        public ActionResult TH1()
        {
            return View();
        }
        /// <summary>
        /// 供应商退货列表查询
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
        public JsonResult ListTH(int page, int rows, string starttime, string endtime, string OrderNum, string ProductName, string username)
        {
            THQuery fpq = new THQuery();
            long inti;
            if (long.TryParse(OrderNum, out inti))
            {
                fpq.orderNum = long.Parse(OrderNum);
            }
            fpq.productName = ProductName;
            fpq.muserid = base.CurrentUser.Id;
            fpq.musername = base.CurrentUser.UserName;
            fpq.musertype = base.CurrentUser.UserType;
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
            fpq.muserid = base.CurrentUser.Id;
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
        /// 同意退货
        /// </summary>
        /// <param name="thnum">退货单号</param>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        public JsonResult THTY(string thnum, string orderid)
        {
            if (string.IsNullOrWhiteSpace(thnum))
            {
                return Json("no");
            }
            TH th = ServiceHelper.Create<ITHService>().GetTHInfo(thnum);
            if (th == null)
            {
                return Json("no");
            }
            th.TH_Status = 6;
            if (ServiceHelper.Create<ITHService>().UpdateTH(th))
            {
                Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                if (fwinfo == null)
                {
                    return Json("no");
                }
                //更新供应商的锁定金额 减去退款金额
                fwinfo.Wallet_UserMoneyLock = fwinfo.Wallet_UserMoneyLock - th.TH_ProductMoneyReal;
                if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                {
                    #region 添加退款信息
                    OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
                    Finance_Refund frinfo = new Finance_Refund();
                    ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                    long frid = _orderBO.GenerateOrderNumber();
                    frinfo.Refund_Number = frid;
                    frinfo.Refund_OrderNum = long.Parse(thnum);
                    frinfo.Refund_UserId = base.CurrentUser.Id;
                    frinfo.Refund_UserType = base.CurrentUser.UserType;
                    frinfo.Refund_UserName = base.CurrentUser.UserName;
                    frinfo.Refund_ToUserId = oinfo.UserId;
                    frinfo.Refund_ToUserType = 3;
                    frinfo.Refund_ToUserName = oinfo.UserName;
                    frinfo.Refund_Money = th.TH_ProductMoneyReal;
                    frinfo.Refund_MoneyType = th.TH_ProductMoneyType;
                    frinfo.Refund_SXMoney = 0;
                    frinfo.Refund_ISChujing = 0;
                    frinfo.Refund_Address = ChemCloud.Core.Common.GetIpAddress();
                    frinfo.Refund_Time = DateTime.Now;
                    frinfo.Refund_Status = 1;
                    if (ServiceHelper.Create<IFinance_RefundService>().AddFinance_Refund(frinfo))
                    {
                        //更新采购商的可用余额  返还退款金额
                        Finance_Wallet fwuinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(oinfo.UserId, 3, int.Parse(oinfo.CoinType.ToString()));
                        fwuinfo.Wallet_UserLeftMoney = fwuinfo.Wallet_UserLeftMoney + th.TH_ProductMoneyReal;
                        if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwuinfo))
                        {
                            //更新订单状态 已退货
                            ServiceHelper.Create<IOrderService>().UpdateOrderStatu(long.Parse(orderid), 9);
                            return Json("yes");
                        }
                        else
                        {
                            return Json("no");
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
                    return Json("no");
                }
            }
            else
            {
                return Json("no");
            }
        }
        /// <summary>
        /// 拒绝退货
        /// </summary>
        /// <param name="thnum"></param>
        /// <returns></returns>
        public JsonResult THJJ(string thnum)
        {
            if (string.IsNullOrWhiteSpace(thnum))
            {
                return Json("no");
            }
            TH th = ServiceHelper.Create<ITHService>().GetTHInfo(thnum);
            if (th == null)
            {
                return Json("no");
            }
            th.TH_Status = 7;
            if (ServiceHelper.Create<ITHService>().UpdateTH(th))
            {
                return Json("yes");
            }
            else
            {
                return Json("no");
            }
        }

        public ActionResult TK()
        {
            return View();
        }
        public ActionResult TK1()
        {
            return View();
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
        public JsonResult ListTK(int page, int rows, string starttime, string endtime, string OrderNum, string username)
        {
            Finance_RefundQuery fpq = new Finance_RefundQuery();
            long inti;
            if (long.TryParse(OrderNum, out inti))
            {
                fpq.orderNum = long.Parse(OrderNum);
            }
            fpq.musername = username;
            fpq.userid = base.CurrentUser.Id;
            fpq.usertype = base.CurrentUser.UserType;
            fpq.starttime = starttime;
            fpq.endtime = endtime;
            fpq.PageSize = rows;
            fpq.PageNo = page;
            PageModel<Finance_Refund> fp = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundListInfo(fpq);
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
        /// 同意退款
        /// </summary>
        /// <param name="thnum">退款单号</param>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        public JsonResult TKTY(string thnum, string orderid)
        {
            if (string.IsNullOrWhiteSpace(thnum))
            {
                return Json("no");
            }
            Finance_Refund fr = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundInfo(long.Parse(thnum));
            if (fr == null)
            {
                return Json("no");
            }
            //更新采购商可用余额 增加退款金额
            OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderid));
            Finance_Wallet fwuserinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(oinfo.UserId, 3, int.Parse(oinfo.CoinType.ToString()));
            fwuserinfo.Wallet_UserLeftMoney = fwuserinfo.Wallet_UserLeftMoney + fr.Refund_Money;
            if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwuserinfo))
            {
                //更新供应商的锁定金额  减去退款金额
                Finance_Wallet fwinfo = ServiceHelper.Create<IFinance_WalletService>().GetWalletInfo(base.CurrentUser.Id, base.CurrentUser.UserType, int.Parse(ConfigurationManager.AppSettings["CoinType"]));
                fwinfo.Wallet_UserMoneyLock = fwinfo.Wallet_UserMoneyLock - fr.Refund_Money;
                if (ServiceHelper.Create<IFinance_WalletService>().UpdateFinance_Wallet(fwinfo))
                {
                    //更新退款状态
                    fr.Refund_Status = 1;
                    if (ServiceHelper.Create<IFinance_RefundService>().UpdateFinance_Refund(fr))
                    {
                        //更新订单状态 已退款
                        ServiceHelper.Create<IOrderService>().UpdateOrderStatu(long.Parse(orderid), 11);
                        return Json("yes");
                    }
                    else
                    {
                        return Json("no");
                    }
                }
                else
                {
                    return Json("no");
                }
            }
            else
            {
                return Json("no");
            }
        }

        public JsonResult TKJJ(string thnum) {
            if (string.IsNullOrWhiteSpace(thnum))
            {
                return Json("no");
            }
            Finance_Refund th = ServiceHelper.Create<IFinance_RefundService>().GetFinance_RefundInfo(long.Parse(thnum));
            if (th == null)
            {
                return Json("no");
            }
            th.Refund_Status = 2;
            if (ServiceHelper.Create<IFinance_RefundService>().UpdateFinance_Refund(th))
            {
                OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(th.Refund_OrderNum);
                oinfo.OrderStatus = OrderInfo.OrderOperateStatus.Finish;
                ServiceHelper.Create<IOrderService>().UpdateOrderStatu(th.Refund_OrderNum, 5);               
                return Json("yes");
            }
            else
            {
                return Json("no");
            }
        }
    }
}