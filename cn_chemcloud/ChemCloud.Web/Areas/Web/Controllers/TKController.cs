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

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class TKController : BaseWebController
    {
        // GET: Web/TK
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
            PageModel<TK> tk = ServiceHelper.Create<ITKService>().getTkList(tq, base.CurrentUser.Id, 0);
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
                    SellerUserId = item.SellerUserId,
                    SellerName = ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(item.SellerUserId) == null ? "" : ServiceHelper.Create<IManagerService>().GetMemberIdByShopId(item.SellerUserId).UserName
                };
            return Json(new { rows = array, total = tk.Total });
        }

        public ActionResult Step1(string orderNo)
        {
            ViewBag.orderNo = orderNo;
            return View();
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
                ViewBag.ReasonType = tk.ReasonType;
                ViewBag.TKInstruction = tk.TKInstruction;
            }
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

        /// <summary>
        /// 供应商退款方法
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="Reson"></param>
        /// <param name="Amont"></param>
        /// <param name="Introduce"></param>
        /// <param name="ReasonType"></param>
        /// <param name="images"></param>
        /// <param name="Attitude"></param>
        /// <returns></returns>
        public JsonResult InsertTK(long OrderNo, string Reson, string Amont, string Introduce, int ReasonType, string images, string Attitude)
        {
            Result res = new Result();
            try
            {
                OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(OrderNo));
                DateTime end = DateTime.Now.AddDays(3);
                TK tkQ = ServiceHelper.Create<ITKService>().getTK(OrderNo);
                long TKid = 0;
                if (tkQ == null)
                {
                    /*1.添加退款信息*/
                    TK tk = new TK()
                    {
                        TKDate = DateTime.Now,
                        OrderId = Convert.ToInt64(OrderNo),
                        TKAmont = Convert.ToDecimal(Amont),
                        EndDate = end,
                        TKInstruction = Introduce,
                        TKResion = Reson,
                        TKType = 1, /*1退款中*/
                        SellerUserId = order.ShopId,
                        UserId = base.CurrentUser.Id,
                        ReasonType = ReasonType
                    };
                    TKid = ServiceHelper.Create<ITKService>().InsertTK(tk);

                    /*订单状态为 退款中*/
                    ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 10);
                }
                else
                {
                    TKid = tkQ.Id;
                    /*再次提交拒绝退款*/
                    if (tkQ.TKType == 4)
                    {
                        tkQ.TKType = 1;
                        ServiceHelper.Create<ITKService>().UpdateTK(tkQ.OrderId, 1);
                        /*订单状态为 退款中*/
                        ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 10);
                    }
                }


                /*2.添加退款日志*/
                TKMessage tkm = new TKMessage()
                {
                    MessageContent = Introduce,
                    MessageDate = DateTime.Now,
                    MessageAttitude = Convert.ToInt32(Attitude),
                    ReturnName = base.CurrentUser.RealName,
                    UserId = base.CurrentUser.Id,
                    TKId = TKid
                };
                long tkmid = ServiceHelper.Create<ITKService>().InsertTKMessage(tkm);

                /*3、退款凭证*/
                string[] imgs = images.Split(',');
                List<TKImageInfo> tkis = new List<TKImageInfo>();
                foreach (string item in imgs)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }
                    TKImageInfo tki = new TKImageInfo()
                    {
                        TKImage = item,
                        TKMessageId = tkmid
                    };
                    tkis.Add(tki);
                }
                ServiceHelper.Create<ITKService>().InsertTKImage(tkis);

                res.success = true;
                res.msg = "操作成功！";

            }
            catch (Exception)
            {
                res.success = false;
                res.msg = "操作失败！";
            }
            return Json(res);
        }
        public ActionResult Step3(long OrderNo)
        {

            TK tk = ServiceHelper.Create<ITKService>().getTK(OrderNo);
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
                ViewBag.ReasonType = tk.ReasonType;
                ViewBag.TKInstruction = tk.TKInstruction;

                ViewBag.TK = tk.EndDate.ToString("yyyy-MM-dd hh:mm:ss");
                List<TKMessageModel> tkmms = new List<TKMessageModel>();
                List<TKMessage> tks = ServiceHelper.Create<ITKService>().getTKMessage(tk.Id);
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
                }
                ViewBag.tkmms = tkmms;
            }
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

            return View();
        }
        public JsonResult ChangeType(long id, int type)
        {

            Result res = new Result();

            if (type == 6)
            {
                /*订单状态为 取消退货 状态改为已支付*/
                ServiceHelper.Create<IOrderService>().UpdateOrderStatu(id, 2);
            }
            ServiceHelper.Create<ITKService>().DeleteTK(id);
            res.success = true;
            return Json(res);
        }
    }
}