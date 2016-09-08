using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class TKController : BaseSellerController
    {
        // GET: SellerAdmin/TK
        public ActionResult Management()
        {
            return View();
        }
        public JsonResult List(int page, int rows, string OrderNo, int Type)
        {
            if (OrderNo == "")
            {
                OrderNo = "0";
            }
            TKQuery tq = new TKQuery()
            {
                TKType = Type,
                PageNo = page,
                PageSize = rows,
                OrderNo = Convert.ToInt64(OrderNo)
            };
            PageModel<TK> tk = ServiceHelper.Create<ITKService>().getTkList(tq, 0, base.CurrentSellerManager.ShopId);
            var array =
                from item in tk.Models.ToArray()
                select new { Id = item.Id, OrderId = item.OrderId, TKType = item.TKType, TKAmont = item.TKAmont, TKDate = item.TKDate, EndDate = item.EndDate, TKResion = item.TKResion };
            return Json(new { rows = array, total = tk.Total });
        }

        public JsonResult ListTH(int page, int rows, string OrderNo, int Type)
        {
            if (OrderNo == "")
            {
                OrderNo = "0";
            }
            THQuery tq = new THQuery()
            {
                Status = Type,
                PageNo = page,
                PageSize = rows,
                orderNum = Convert.ToInt64(OrderNo),
                muserid = int.Parse(base.CurrentUser.Id.ToString())
            };
            PageModel<TH> tk = ServiceHelper.Create<ITHService>().GetTHListInfo1(tq);

            return Json(new { rows = tk.Models.ToArray(), total = tk.Total });
        }

        /// <summary>
        /// 退款页面
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult Step2(long orderNo)
        {
            TK tk = ServiceHelper.Create<ITKService>().getTK(orderNo);
            if (tk == null)
            {
                ViewBag.TKResion = 0;
                ViewBag.TKAmont = "";
                ViewBag.type = 0;
                ViewBag.TKInstruction = "";
            }
            else
            {
                ViewBag.TKResion = tk.TKResion;
                ViewBag.TKAmont = tk.TKAmont;
                ViewBag.type = tk.TKType;
                ViewBag.TKInstruction = tk.TKInstruction;
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
            if (order != null && order.ShopId == base.CurrentSellerManager.ShopId)
            {
                ViewBag.OrderNo = orderNo;
                ViewBag.ShopName = order.UserName;
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

        /// <summary>
        /// 退款操作
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <param name="Reson">退款原因</param>
        /// <param name="Amont">金额</param>
        /// <param name="Introduce">回复</param>
        /// <param name="images">凭证(暂未用)</param>
        /// <param name="Attitude">行为1同意2拒绝</param>
        /// <returns></returns>
        public JsonResult InsertTK(long OrderNo, string Reson, string Amont, string Introduce, string images, int Attitude)
        {
            Result res = new Result();
            try
            {
                //订单信息
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(OrderNo));

                //退款单信息
                TK tkQ = ServiceHelper.Create<ITKService>().getTK(OrderNo);

                //添加退款日志信息
                TKMessage tkm = new TKMessage()
                {
                    MessageContent = Introduce,
                    MessageDate = DateTime.Now,
                    MessageAttitude = Convert.ToInt32(Attitude),
                    ReturnName = base.CurrentUser.RealName,
                    UserId = base.CurrentUser.Id,
                    TKId = tkQ.Id
                };
                long tkmid = ServiceHelper.Create<ITKService>().InsertTKMessage(tkm);

                if (Attitude == 1)
                {
                    //同意退款 
                    //1、更改退货单状态为 已同意
                    ServiceHelper.Create<ITKService>().UpdateTK(OrderNo, 3);
                    //更改订单状态为退款中，等待后台审核 转账
                    ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 10);
                }
                else
                {
                    //拒绝退款 
                    //1、更改退货单状态为 拒绝退款
                    ServiceHelper.Create<ITKService>().UpdateTK(OrderNo, 4);
                    //更改订单状态为拒绝退款
                    ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 12);
                }
                res.success = true;
                res.msg = "操作成功！";
            }
            catch (Exception)
            {
                res.success = false;
                res.msg = "操作异常！";
            }
            return Json(res);
        }

        /*退货页面*/
        public ActionResult Step3(long OrderNo)
        {
            TH tk = ServiceHelper.Create<ITHService>().GetTHByOrderNum(OrderNo);
            if (tk == null)
            {
                ViewBag.TKResion = 0;
                ViewBag.TKAmont = "";
                ViewBag.type = 0;

                tk.TH_WLDH = "";
                tk.TH_WLGS = "";
            }
            else
            {
                ViewBag.TKResion = tk.TH_Reason;
                ViewBag.TKAmont = tk.TH_ProductMoney;
                ViewBag.type = tk.TH_Status;
            }

            List<THMessageModel> tkmms = new List<THMessageModel>();
            List<THMessageInfo> tks = ServiceHelper.Create<ITHService>().getTHMessage(tk.Id);
            if (tks != null)
            {
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
                    ViewBag.MessageContent += "" + item.MessageContent;
                }
                ViewBag.tkmms = tkmms;
            }

            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(OrderNo));
            if (order != null && order.ShopId == base.CurrentSellerManager.ShopId)
            {
                ViewBag.OrderNo = OrderNo;
                ViewBag.ShopName = order.UserName;
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

            return View(tk);
        }


        /*退货审核操作*/
        public JsonResult InsertTH(long OrderNo, string Introduce, int Attitude)
        {
            Result res = new Result();
            try
            {
                //订单信息
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(OrderNo));

                //退款单信息
                TH tkQ = ServiceHelper.Create<ITHService>().GetTHByOrderNum(OrderNo);

                //添加退款日志信息
                THMessageInfo tkm = new THMessageInfo()
                {
                    MessageContent = Introduce,
                    MessageDate = DateTime.Now,
                    MessageAttitude = Convert.ToInt32(Attitude),
                    ReturnName = base.CurrentUser.RealName,
                    UserId = base.CurrentUser.Id,
                    THId = tkQ.Id
                };
                long tkmid = ServiceHelper.Create<ITHService>().InsertTHMessage(tkm);

                if (Attitude == 1)
                {
                    //同意退款 
                    //1、更改退货单状态为 已同意
                    ServiceHelper.Create<ITHService>().UpdateTHStatus(OrderNo, 2);
                    //更改订单状态为退货中，等待后台审核 转账
                    ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 8);
                }
                else
                {
                    //拒绝退款 
                    //1、更改退货单状态为 拒绝退货
                    ServiceHelper.Create<ITHService>().UpdateTHStatus(OrderNo, 3);
                    //更改订单状态为拒绝退货
                    ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 13);
                }
                res.success = true;
                res.msg = "操作成功！";
            }
            catch (Exception)
            {
                res.success = false;
                res.msg = "操作异常！";
            }
            return Json(res);
        }

        /*更改退货单状态*/
        public JsonResult ChangeTHStatus(long id, int type)
        {
            Result res = new Result();

            ServiceHelper.Create<ITHService>().UpdateTHStatus(id, type);

            res.success = true;
            return Json(res);
        }
    }
}