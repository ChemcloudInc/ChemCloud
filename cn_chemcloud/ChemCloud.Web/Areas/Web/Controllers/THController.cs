using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.QueryModel;
using System.Configuration;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class THController : BaseWebController
    {
        // GET: Web/TH
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Management()
        {
            return View();
        }

        /*查询TH列表*/
        public JsonResult List(int page, int rows, string OrderNo, int Type)
        {
            /* 卖家同意退货后，买家可寄回货填写订单号，卖家收到货确认签收，
             * 平台审核完成 并且从供应商扣款给采购商
            <option value="0">请选择</option>
            <option value="1">退货申请中</option>
            <option value="2">同意退货</option>
            <option value="3">拒绝退货</option>
            <option value="4">已寄货</option>
            <option value="5">已签收</option>
            <option value="6">已完成</option>
            <option value="7">仲裁中</option>
             */

            /*Type 退货状态*/
            if (OrderNo == "")
            {
                OrderNo = "0";
            }
            THQuery tq = new THQuery()
            {
                Status = Type,
                PageNo = page,
                PageSize = rows,
                orderNum = Convert.ToInt64(OrderNo)
            };
            PageModel<TH> tk = ServiceHelper.Create<ITHService>().GetTHPageModel(tq, base.CurrentUser.Id, 0);
            var array =
                from item in tk.Models.ToArray()
                select new
                {
                    Id = item.Id,
                    TH_Number = item.TH_Number,
                    TH_OrderNum = item.TH_OrderNum,
                    TH_Time = item.TH_Time,
                    TH_UserId = item.TH_UserId,
                    TH_UserName = item.TH_UserName,
                    TH_UserType = item.TH_UserType,
                    TH_ProductName = item.TH_ProductName,
                    TH_ProductCount = item.TH_ProductCount,
                    TH_ProductMoney = item.TH_ProductMoney,
                    TH_ProductMoneyReal = item.TH_ProductMoneyReal,
                    TH_ProductMoneyType = item.TH_ProductMoneyType,
                    TH_ToUserId = item.TH_ToUserId,
                    TH_ToUserName = item.TH_ToUserName,
                    TH_ToUserType = item.TH_ToUserType,
                    TH_Status = item.TH_Status,
                    TH_Reason = item.TH_Reason
                };
            return Json(new { rows = array, total = tk.Total });
        }

        public ActionResult Step1(string orderNo)
        {
            ViewBag.orderNo = orderNo;
            return View();
        }

        public ActionResult Step2(long orderNo)
        {
            /*退货信息*/
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
            }
            else
            {
                ViewBag.TH_Reason = th.TH_Reason;
                ViewBag.TH_ProductMoney = th.TH_ProductMoney;
                ViewBag.TH_Status = th.TH_Status;
            }

            /*订单信息*/
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(orderNo));
            if (order.UserId == base.CurrentUser.Id)
            {
                ViewBag.OrderNo = orderNo;
                ViewBag.ShopName = order.ShopName;
                ViewBag.yunfei = order.Freight;
                ViewBag.total = order.ProductTotalAmount;
                ViewBag.max = order.Freight + order.ProductTotalAmount;
                ViewBag.ShopId = order.ShopId;
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
            return View();
        }

        public ActionResult Step3(long OrderNo)
        {

            TH th = ServiceHelper.Create<ITHService>().GetTHByOrderNum(OrderNo);
            if (th == null)
            {
                decimal newmoney = 0;
                OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(OrderNo);
                OrderItemInfo oiiinfo = ServiceHelper.Create<IOrderService>().GetOrderItemInfo(OrderNo);
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
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(OrderNo));
            if (order.UserId == base.CurrentUser.Id)
            {
                ViewBag.OrderNo = OrderNo;
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
            ViewBag.OrderNo = OrderNo;

            return View(th);
        }

        /*采购商退货*/
        public JsonResult InsertTH(long OrderNo, string Reson, string Amont, string Introduce, string images, string Attitude)
        {
            Result res = new Result();
            try
            {
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(OrderNo));

                TH thq = ServiceHelper.Create<ITHService>().GetTHByOrderNum(OrderNo);
                long TKid = 0;
                if (thq == null)
                {
                    OrderInfo oinfo = ServiceHelper.Create<IOrderService>().GetOrder(OrderNo);
                    OrderItemInfo oiiinfo = ServiceHelper.Create<IOrderService>().GetOrderItemInfo(OrderNo);
                    if (oiiinfo == null || oinfo == null)
                    {
                        res.success = false;
                    }
                    ChemCloud.Service.Order.Business.OrderBO _orderBO = new ChemCloud.Service.Order.Business.OrderBO();
                    TH th = new TH();
                    var newmoney = oinfo.OrderTotalAmount - (oinfo.Freight + oinfo.Insurancefee + oinfo.Transactionfee + oinfo.Counterfee);
                    th.TH_Number = _orderBO.GenerateOrderNumber();
                    th.TH_OrderNum = OrderNo;
                    th.TH_Time = DateTime.Now;
                    th.TH_UserId = base.CurrentUser.Id;
                    th.TH_UserName = base.CurrentUser.UserName;
                    th.TH_UserType = base.CurrentUser.UserType;
                    th.TH_ProductName = oiiinfo.ProductName;
                    th.TH_ProductCount = int.Parse(oiiinfo.Quantity.ToString());
                    th.TH_ProductMoney = newmoney;
                    th.TH_ProductMoneyReal = newmoney;
                    th.TH_ProductMoneyType = int.Parse(ConfigurationManager.AppSettings["CoinType"]);
                    ManagerInfo minfo = ServiceHelper.Create<IManagerService>().GetManagerInfoByShopId(oinfo.ShopId);
                    UserMemberInfo uminfo = ServiceHelper.Create<IMemberService>().GetMemberByName(minfo.UserName);
                    th.TH_ToUserId = uminfo.Id;
                    th.TH_ToUserName = oinfo.ShopName;
                    th.TH_ToUserType = uminfo.UserType;
                    th.TH_Status = 1;
                    th.TH_Reason = Reson;

                    TKid = ServiceHelper.Create<ITHService>().AddTH(th);
                    if (TKid != 0)
                    {
                        oinfo.OrderStatus = ChemCloud.Model.OrderInfo.OrderOperateStatus.THing;
                        /*订单状态为 退货中*/
                        ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 8);
                        /*2.添加退货日志*/
                        THMessageInfo tkm = new THMessageInfo()
                        {
                            MessageContent = Introduce,
                            MessageDate = DateTime.Now,
                            MessageAttitude = Convert.ToInt32(Attitude),
                            ReturnName = base.CurrentUser.RealName,
                            UserId = base.CurrentUser.Id,
                            THId = TKid
                        };
                        long thmid = ServiceHelper.Create<ITHService>().InsertTHMessage(tkm);

                        /*3、退货凭证*/
                        string[] imgs = images.Split(',');
                        List<THImageInfo> tkis = new List<THImageInfo>();
                        foreach (string item in imgs)
                        {
                            if (string.IsNullOrWhiteSpace(item))
                            {
                                continue;
                            }
                            THImageInfo tki = new THImageInfo()
                            {
                                THImage = item,
                                THMessageId = thmid
                            };
                            tkis.Add(tki);
                        }
                        ServiceHelper.Create<ITHService>().InsertTHImage(tkis);

                        res.success = true;
                        res.msg = "操作成功！";
                    }
                    else
                    {
                        res.success = false;
                        res.msg = "操作失败！";
                    }
                }
                else
                {
                    TKid = thq.Id;

                    if (thq.TH_Status == 3)
                    {
                        /*更改退货订单为 退货申请中1  */
                        ServiceHelper.Create<ITHService>().UpdateTHStatus(thq.TH_OrderNum, 1);
                        /*订单状态为 退货中*/
                        ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 8);
                    }

                    /*2.添加退货日志*/
                    THMessageInfo tkm = new THMessageInfo()
                    {
                        MessageContent = Introduce,
                        MessageDate = DateTime.Now,
                        MessageAttitude = Convert.ToInt32(Attitude),
                        ReturnName = base.CurrentUser.RealName,
                        UserId = base.CurrentUser.Id,
                        THId = TKid
                    };
                    long thmid = ServiceHelper.Create<ITHService>().InsertTHMessage(tkm);

                    /*3、退货凭证*/
                    string[] imgs = images.Split(',');
                    List<THImageInfo> tkis = new List<THImageInfo>();
                    foreach (string item in imgs)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }
                        THImageInfo tki = new THImageInfo()
                        {
                            THImage = item,
                            THMessageId = thmid
                        };
                        tkis.Add(tki);
                    }
                    ServiceHelper.Create<ITHService>().InsertTHImage(tkis);

                    res.success = true;
                    res.msg = "操作成功！";
                }

            }
            catch (Exception)
            {
                res.success = false;
                res.msg = "操作失败！";
            }
            return Json(res);
        }

        /*更改退货单状态*/
        public JsonResult ChangeTHStatus(long id, int type)
        {
            Result res = new Result();

            if (type == 8)
            {
                /*订单状态为 取消退货*/
                ServiceHelper.Create<IOrderService>().UpdateOrderStatu(id, 6);
            }

            ServiceHelper.Create<ITHService>().UpdateTHStatus(id, type);

            res.success = true;
            return Json(res);
        }

        /*退货*/
        public JsonResult SendTH(long ordernum, string wuliugongsi, string wuliudanhao)
        {
            Result res = new Result();
            try
            {
                ServiceHelper.Create<ITHService>().SendTH(ordernum, wuliudanhao, wuliugongsi);
                res.success = true;
            }
            catch (Exception)
            {
                res.success = false;
            }
            return Json(res);
        }

    }
}