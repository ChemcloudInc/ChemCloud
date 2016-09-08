using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Payment;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using ChemCloud.Web;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Controllers;
using ChemCloud.Web.Framework;
using com.paypal.sdk.util;
using ChemCloud.AliPay;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Data;
using ChemCloud.DBUtility;
using System.Text;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class OtherOrderController : BaseWebController
    {
        // GET: Web/OtherOrder
        public ActionResult Index()
        {
            return View();
        }

        /*
         * 定制合成
         *
         */

        /// <summary>
        /// 定制合成订单
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomizedOrder()
        {
            if (base.CurrentUser != null) { ViewBag.UserType = base.CurrentUser.UserType; }
            else { ViewBag.UserType = 0; }
            return View();
        }

        /// <summary>
        /// 定制合成订单详细
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomizedOrderDetail(long id)
        {
            OrderSynthesis model = ServiceHelper.Create<IOrderSynthesisService>().GetOrderSynthesis(id);
            model.UserName = ServiceHelper.Create<IMemberService>().GetMember(model.UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(model.UserId).UserName;

            List<AttachmentInfo> models = ServiceHelper.Create<IOrderSynthesisService>().GetAttachmentInfosById(id);
            ViewBag.attachmentInfo = models;

            return View(model);
        }

        /// <summary>
        /// 定制合成订单查询
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="islikevalue"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageno"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchResult(string orderby, int pagesize, int pageno)
        {
            DataSet _ds = GetListByPage(orderby, (pageno - 1) * pagesize + 1, pageno * pagesize);
            if (_ds == null || _ds.Tables[0].Rows.Count < 1)
            {
                return Json("");
            }
            else
            {
                string reslut = ChemCloud.Core.JsonHelper.GetJsonByDataset(_ds);
                return Json(reslut);
            }
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string orderby, int startIndex, int endIndex)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ordertime desc");
            }
            strSql.Append(")AS Row, T.*  from ChemCloud_OrderSynthesis T WHERE T.Status=1 ");

            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());

        }

        /// <summary>
        /// 定制合成订单查询
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="islikevalue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchResultCount()
        {
            string strsql = string.Format("SELECT COUNT(0) AS COUNT FROM ChemCloud_OrderSynthesis WHERE Status=1;");
            DataSet ds = DbHelperSQL.Query(strsql);
            if (ds == null)
            {
                return Json("0");
            }
            else
            {
                return Json(ds.Tables[0].Rows[0]["COUNT"].ToString());
            }
        }

        /// <summary>
        /// 接单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcceptCustomizedOrder(string OrderNumber, string Offer, string OfferCycle)
        {
            try
            {
                /*UserType为2的时候为供应商，只有供应商才可以接单*/
                long memberid = base.CurrentUser.UserType;
                if (memberid == 2)
                {
                    long shopid = base.CurrentSellerManager.ShopId;
                    string strsql = string.Format("  insert into dbo.ChemCloud_OrderSynthesisOffer(OrderNumber,ShopId,Offer,DateTime,OfferCycle) values('" + OrderNumber + "','" + shopid + "','" + Offer + "',GETDATE(),'" + OfferCycle + "')");
                    int result = DbHelperSQL.ExecuteSql(strsql);
                    if (result > 0)
                    {
                        return Json(new { success = true, msg = "报名成功！" });
                    }
                    else
                    {
                        return Json(new { success = false, msg = "报名失败！" });
                    }
                }
                else
                {

                    { return Json(new { success = false, msg = "您还不是供应商！" }); }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "操作异常！" });
            }
        }

        /// <summary>
        /// 统计报名人数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcceptCustomizedOrderCount(string OrderNumber)
        {
            string strsql = string.Format("SELECT a.*,b.ShopName FROM dbo.ChemCloud_OrderSynthesisOffer as a left join ChemCloud_Shops as b on a.ShopId=b.Id where a.OrderNumber='" + OrderNumber + "';");

            DataTable dt = DbHelperSQL.QueryDataTable(strsql);

            if (dt.Rows.Count == 0)
            {
                return Json(new { success = true, msg = "0", jsonstr = "" });
            }
            else
            {
                string json = ChemCloud.Core.JsonHelper.DataTable2Json(dt);

                return Json(new { success = true, msg = dt.Rows.Count.ToString(), jsonstr = json });
            }
        }


        /*
         * 代理采购
         *
        */

        /// <summary>
        /// 代理采购
        /// </summary>
        /// <returns></returns>
        public ActionResult AgentPurchasing()
        {
            if (base.CurrentUser != null) { ViewBag.UserType = base.CurrentUser.UserType; }
            else { ViewBag.UserType = 0; }
            return View();
        }

        public ActionResult AgentPurchasingDetail(long id)
        {
            OrderPurchasing model = ServiceHelper.Create<IOrderPurchasingService>().GetOrderPurchasing(id);
            model.UserName = ServiceHelper.Create<IMemberService>().GetMember(model.UserId) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(model.UserId).UserName;

            List<AttachmentInfo> models = ServiceHelper.Create<IOrderPurchasingService>().GetAttachmentInfosById(id);
            ViewBag.attachmentInfo = models;

            return View(model);
        }

        /// <summary>
        /// 定制合成订单查询
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="islikevalue"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageno"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchResultOrderPurchasing(string orderby, int pagesize, int pageno)
        {
            DataSet _ds = GetListByPageOrderPurchasing(orderby, (pageno - 1) * pagesize + 1, pageno * pagesize);
            if (_ds == null || _ds.Tables[0].Rows.Count < 1)
            {
                return Json("");
            }
            else
            {
                string reslut = ChemCloud.Core.JsonHelper.GetJsonByDataset(_ds);
                return Json(reslut);
            }
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPageOrderPurchasing(string orderby, int startIndex, int endIndex)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ordertime desc");
            }
            strSql.Append(")AS Row, T.*  from ChemCloud_OrderPurchasing T  WHERE T.Status=1 ");

            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());

        }

        /// <summary>
        /// 定制合成订单查询
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="islikevalue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchResultCountOrderPurchasing()
        {
            string strsql = string.Format("SELECT COUNT(0) AS COUNT FROM ChemCloud_OrderPurchasing WHERE Status=1;");
            DataSet ds = DbHelperSQL.Query(strsql);
            if (ds == null)
            {
                return Json("0");
            }
            else
            {
                return Json(ds.Tables[0].Rows[0]["COUNT"].ToString());
            }
        }
    }
}