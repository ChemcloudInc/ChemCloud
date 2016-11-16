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
    public class ProductAnalysisController : BaseWebController
    {
        // GET: Web/ProductAnalysis
        public ActionResult Index()
        {
            return View();
        }

        /*分析鉴定*/
        public ActionResult ProductAnalysis()
        {
            if (base.CurrentUser == null)
            {
                ViewBag.CurrentUser = "0";
            }
            else
            {
                ViewBag.CurrentUser = base.CurrentUser.UserName;
            }
            return View();
        }

        /*分析鉴定提交*/
        [HttpPost]
        public JsonResult AddProductAnalysis(string strjson)
        {
            long memberid = 0;
            if (base.CurrentUser == null)
            {
                base.RedirectToAction("Index", "Login", new { area = "Web" });
            }
            else
            {
                memberid = base.CurrentUser.Id;
            }
            try
            {
                ProductAnalysis _ProductAnalysis = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductAnalysis>(strjson);
                _ProductAnalysis.Memberid = memberid;
                ProductAnalysis resutl = ServiceHelper.Create<IProductAnalysisService>().AddProductAnalysis(_ProductAnalysis);
                if (resutl != null && resutl.Id > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, msg = "failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.ToString() });
            }
        }

        /*查询当前列表*/
        [HttpPost]
        public JsonResult GetProductAnalysisList(int page, int rows)
        {
            ProductAnalysis pQuery = new Model.ProductAnalysis();
            pQuery.PageNo = page;
            pQuery.PageSize = rows;
            pQuery.Memberid = base.CurrentUser.Id;

            PageModel<ProductAnalysis> source = ServiceHelper.Create<IProductAnalysisService>().GetProductAnalysisList(pQuery);
            var array =
                from item in source.Models.ToArray()
                select new
                {
                    Id = item.Id,
                    ClientName = item.ClientName,
                    MemberName = ServiceHelper.Create<IMemberService>().GetMember(item.Memberid) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(item.Memberid).UserName,
                    ReportHeader = item.ReportHeader,
                    SampleName = item.SampleName,
                    SampleQuantity = item.SampleQuantity,
                    SampleSpecifications = item.SampleSpecifications,
                    AnalysisStatus = item.AnalysisStatus,
                    ServiceCharge = item.ServiceCharge,
                    AnalysisAttachments = item.AnalysisAttachments,
                    ClientLinkMan = item.ClientLinkMan
                };
            return Json(new { rows = array, total = source.Total });
        }
    }
}