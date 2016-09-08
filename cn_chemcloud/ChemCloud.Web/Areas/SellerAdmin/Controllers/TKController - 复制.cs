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
                PageNo=page,
                PageSize=rows,
                OrderNo = Convert.ToInt64(OrderNo)
            };
            PageModel<TK> tk = ServiceHelper.Create<ITKService>().getTkList(tq, 0, base.CurrentSellerManager.ShopId);
            var array =
                from item in tk.Models.ToArray()
                select new { Id = item.Id, OrderId = item.OrderId,TKType=item.TKType, TKAmont = item.TKAmont, TKDate = item.TKDate, EndDate = item.EndDate, TKResion = item.TKResion };
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
                orderNum = Convert.ToInt64(OrderNo)
            };
            PageModel<TH> tk = ServiceHelper.Create<ITHService>().GetTHListInfo1(tq);
            
            return Json(new { rows = tk.Models.ToArray(), total = tk.Total });
        }
        public ActionResult Step2(long orderNo)
        {
            TK tk = ServiceHelper.Create<ITKService>().getTK(orderNo);
            if (tk == null)
            {
                ViewBag.TKResion = 0;
                ViewBag.TKAmont = "";
                ViewBag.type = 0;
            }
            else
            {
                ViewBag.TKResion = tk.TKResion;
                ViewBag.TKAmont = tk.TKAmont;
                ViewBag.type = 1;
            }
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(Convert.ToInt64(orderNo));
            List<TKMessageModel> tkmms = new List<TKMessageModel>();
            List<TKMessage> tks = ServiceHelper.Create<ITKService>().getTKMessage(tk.Id);
            if(tks!=null){
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
            if (order!=null&&order.ShopId == base.CurrentSellerManager.ShopId)
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
            if (Attitude == 1)
            {
                //同意退款 更改订单状态为退款中，线下转账
                ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 10);
            }
            else { 
                //拒绝退款，关闭订单
                ServiceHelper.Create<IOrderService>().UpdateOrderStatu(OrderNo, 4);
            }
            Result res = new Result();
            res.success = true;
            res.msg = "提交成功";
            return Json(res);
        }
        public ActionResult Step3(long OrderNo)
        {
            TK tk = ServiceHelper.Create<ITKService>().getTK(OrderNo);
            ViewBag.OrderNo = OrderNo;
            if (tk != null)
            {
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
            return View();
        }
        public ActionResult Step4(long Id)
        {
            TH th = ServiceHelper.Create<ITHService>().GetTHById(Id);
            ViewBag.OrderNo = th.TH_OrderNum;
            if (th != null)
            {
                ViewBag.TK = th.TH_Time.ToString("yyyy-MM-dd hh:mm:ss");
                List<TKMessageModel> tkmms = new List<TKMessageModel>();
                List<TKMessage> tks = ServiceHelper.Create<ITKService>().getTKMessage(th.Id);
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
            return View();
        }

       
    }
}