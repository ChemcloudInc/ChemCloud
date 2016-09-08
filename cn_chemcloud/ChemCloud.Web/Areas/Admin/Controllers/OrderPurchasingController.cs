using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class OrderPurchasingController : Controller
    {
        // GET: Admin/OrderPurchasing
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 平台列表查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int page, int rows, string orderNum, int orderStatus, string startTime, string endTime)
        {
            OrderPurchasingQuery opQuery = new OrderPurchasingQuery();
            DateTime dt;
            if (DateTime.TryParse(startTime, out dt) && DateTime.TryParse(endTime, out dt))
            {
                opQuery = new OrderPurchasingQuery()
                {
                    PageSize = rows,
                    PageNo = page,
                    beginTime = DateTime.Parse(startTime),
                    endTime = DateTime.Parse(endTime),
                    status = orderStatus,
                    orderNum = orderNum
                };
            }
            else
            {
                opQuery = new OrderPurchasingQuery()
                {
                    PageSize = rows,
                    PageNo = page,
                    status = orderStatus,
                    orderNum = orderNum
                };
            }
            PageModel<OrderPurchasing> opModel = ServiceHelper.Create<IOrderPurchasingService>().GetOrderPurchasingList(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, ProductName = item.ProductName, OrderNum = item.OrderNum, ProductPrice = item.ProductPrice, Status = item.Status, ProductCount = item.ProductCount, Email = item.Email, CompanyName = item.CompanyName };
            return Json(new { rows = array, total = opModel.Total });
        }
        /// <summary>
        /// 平台订单详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult manage(long id)
        {
            OrderPurchasing order = ServiceHelper.Create<IOrderPurchasingService>().GetOrderPurchasing(id);
            List<AttachmentInfo> models = ServiceHelper.Create<IOrderSynthesisService>().GetAttachmentInfosById(id, 5);
            ViewBag.parentId = id;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View(order);
        }
        /// <summary>
        /// 平台修改详细信息
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult ManUpdate(long id, int status, string price = "", string reply = "", string exname = "", string exid = "", string deliverdate = "", string cost = "")
        {
            OrderPurchasing order = ServiceHelper.Create<IOrderPurchasingService>().GetOrderPurchasing(id);
            if (order == null)
            {
                return Json(new { Successful = false });
            }
            order.Status = status;
            if (!string.IsNullOrWhiteSpace(price))
            {
                order.ProductPrice = price;
            }
            if (!string.IsNullOrWhiteSpace(reply))
            {
                order.ReplyContent = reply;
            }
            if (!string.IsNullOrWhiteSpace(exname))
            {
                order.KuaiDi = exname;
            }
            if (!string.IsNullOrWhiteSpace(exid))
            {
                order.KuaiDiNo = exid;
            }

            if (!string.IsNullOrWhiteSpace(deliverdate))
            {
                order.DeliveryDate = DateTime.Parse(deliverdate);
            }
            if (!string.IsNullOrWhiteSpace(cost))
            {
                order.Cost = decimal.Parse(cost);
            }


            ServiceHelper.Create<IOrderPurchasingService>().UpdateOrderPurchasing(order);
            return Json(new { Successful = true });
        }

        [HttpPost]
        public JsonResult GetExpressData(string shipOrderNumber)
        {
            if (string.IsNullOrWhiteSpace(shipOrderNumber))
            {
                throw new HimallException("错误的订单信息");
            }
            OrderExpressQuery oe = ServiceHelper.Create<IOrderExpressQueryService>().GetOrderExpressById(shipOrderNumber);
            return Json(oe);
        }
    }
}
