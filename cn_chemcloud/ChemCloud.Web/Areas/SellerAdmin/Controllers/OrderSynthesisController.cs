using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.DBUtility;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
using ChemCloud.QueryModel;
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

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class OrderSynthesisController : BaseSellerController
    {
        // GET: SellerAdmin/OrderSynthesis
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 定制合成表查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int page, int rows, string orderNum, int orderStatus)
        {
            OrderSynthesisQuery opQuery = new OrderSynthesisQuery();

            opQuery = new OrderSynthesisQuery()
            {
                PageSize = rows,
                PageNo = page,
                status = orderStatus,
                OrderNumber = orderNum,
                ShopId = base.CurrentSellerManager.ShopId.ToString()
            };

            PageModel<OrderSynthesis> opModel = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesisList(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    OrderNum = item.OrderNumber,
                    //ProductPrice = item.Price,
                    ProductPrice = (decimal.Parse(item.Price) * decimal.Parse(item.ProductCount)).ToString("0.00"),
                    Status = item.Status,
                    ProductCount = item.ProductCount,
                    Email = item.Email,
                    CompanyName = item.CompanyName
                };
            return Json(new { rows = array, total = opModel.Total });
        }
        /// <summary>
        /// 定制合成详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(long id, string sid)
        {
            OrderSynthesis order = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            List<AttachmentInfo> models = ServiceHelper.Create<IOrderSynthesisService>().GetAttachmentInfosById(id);
            ViewBag.parentId = id;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View(order);
        }

        /// <summary>
        ///  定制合成-供应商发货
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult DetailUpdate(long id, int status, string exname = "", string exid = "")
        {
            OrderSynthesis order = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            if (order == null)
            {
                return Json(new { success = false });
            }
            else
            {
                try
                {
                    order.Status = status;
                    order.ExpressConpanyName = exname;
                    order.ShipOrderNumber = exid;

                    if (id > 0)
                    {
                        OrderSynthesis querymodel = new OrderSynthesis();
                        querymodel.Id = id;
                        querymodel.Status = status;
                        querymodel.ExpressConpanyName = exname;
                        querymodel.ShipOrderNumber = exid;
                        if (ServiceHelper.Create<IOrderSynthesisService>().OrderSynthesis_DeliverGoods(querymodel))
                        {
                            ServiceHelper.Create<ISiteMessagesService>().SendSiteMessages(order.UserId, (int)MessageSetting.MessageModuleStatus.OrderShipping, "您的订单已发货，请查看。", "chemcloud");

                            return Json(new { success = true, msg = "操作成功！" });
                        }
                        else
                        {
                            return Json(new { success = false, msg = "操作失败！" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, msg = "操作失败！" });
                    }
                    /*
                    string strsql = string.Format("update dbo.ChemCloud_OrderSynthesis set Status='" + status + "',ExpressConpanyName='" + exname + "',ShipOrderNumber='" + exid + "'  where Id='" + id + "';");
                    int result = DbHelperSQL.ExecuteSql(strsql);
                    if (result > 0)
                    {
                        ServiceHelper.Create<ISiteMessagesService>().SendSiteMessages(order.UserId, (int)MessageSetting.MessageModuleStatus.OrderShipping, "您的订单已发货，请查看。", "chemcloud");
                        return Json(new { success = true, msg = "操作成功！" });
                    }
                    else
                    {
                        return Json(new { success = false, msg = "操作失败！" });
                    }
                     */
                }
                catch (Exception)
                {
                    return Json(new { success = false });
                }
            }
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