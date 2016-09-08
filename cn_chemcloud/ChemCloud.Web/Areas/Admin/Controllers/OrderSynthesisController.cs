using ChemCloud.Model.Common;
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
    public class OrderSynthesisController : Controller
    {
        // GET: Admin/OrderSynthesis
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 平台列表查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int page, int rows, string orderNum, int orderStatus)
        {
            OrderSynthesisQuery opQuery = new OrderSynthesisQuery();
            DateTime dt;

            opQuery = new OrderSynthesisQuery()
            {
                PageSize = rows,
                PageNo = page,

                status = orderStatus,
                OrderNumber = orderNum
            };

            PageModel<OrderSynthesis> opModel = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesisList(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, ProductName = item.ProductName, OrderNum = item.OrderNumber, ProductPrice = item.Price, Status = item.Status, ProductCount = item.ProductCount, Email = item.Email, CompanyName = item.CompanyName };
            return Json(new { rows = array, total = opModel.Total });
        }
        /// <summary>
        /// 平台订单详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult manage(long id, string sid)
        {
            OrderSynthesis order = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            List<AttachmentInfo> models = ServiceHelper.Create<IOrderSynthesisService>().GetAttachmentInfosById(id);
            ViewBag.parentId = id;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View(order);
        }
        /// <summary>
        /// 平台审核定制合成订单
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult ManUpdate(long id, int status = 1, string reply = "")
        {
            OrderSynthesis order = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            if (order == null)
            {
                return Json(new { Successful = false });
            }
            order.Status = status;

            if (!string.IsNullOrWhiteSpace(reply))
            {
                order.OperatorMessage = reply;
            }

            ServiceHelper.Create<IOrderSynthesisService>().UpdateOrderSynthesis(order);
            string messagecontent = "chemcloud确认了定制合成订单：" + id + "，请查看。";
            ServiceHelper.Create<ISiteMessagesService>().SendSiteMessages(order.UserId, (int)MessageSetting.MessageModuleStatus.OrderCreated, messagecontent, "chemcloud");
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


        [HttpPost]
        public Result_List<OrderSynthesis_Index> GetTopNumOrderSynthesis(int count)
        {
            Result_List<OrderSynthesis_Index> list = ServiceHelper.Create<IOrderSynthesisService>().GetTopNumOrderSynthesis(count);
            return list;
        }
    }
}
