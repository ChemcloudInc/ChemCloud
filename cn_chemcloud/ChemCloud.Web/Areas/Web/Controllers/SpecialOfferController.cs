using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.Web.Areas.Web;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.Web.Models;
using System.Configuration;
using ChemCloud.Model.Common;
using System.Web.Services;
using System.Data;
using System.Text;
using ChemCloud.DBUtility;
using System.Text.RegularExpressions;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class SpecialOfferController : BaseWebController
    {
        // GET: Web/SpecialOffer
        public ActionResult Index()
        {
            if (base.CurrentUser != null)
            {
                ViewBag.UserType = base.CurrentUser.UserType;
            }
            else
            {
                ViewBag.UserType = 0;
            }
            return View();
        }

        [HttpPost]
        public JsonResult SearchResult(int pagesize, int pageno)
        {
            DataSet _ds = GetListByPage((pageno - 1) * pagesize + 1, pageno * pagesize);
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
        public DataSet GetListByPage(int startIndex, int endIndex)
        {
            string strSql = "SELECT * FROM (SELECT ROW_NUMBER() OVER( ORDER BY T.AddedDate)AS Row, T.*,b.Description,c.ShopName from ChemCloud_Products T left join ChemCloud_ProductDescriptions as b on T.Id=b.ProductId left join ChemCloud_Shops as c on  T.ShopId=c.Id  where T.Weight=3 AND T.Volume=3 )TT WHERE TT.Row BETWEEN " + startIndex + " and " + endIndex + "";

            return DbHelperSQL.Query(strSql);
        }
        [HttpPost]
        public JsonResult SearchResultCount()
        {
            /*sql查询*/
            string strsql = string.Format("SELECT COUNT(0) AS COUNT FROM ChemCloud_Products WHERE Weight=3 AND Volume=3 ");
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