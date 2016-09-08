using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Web.Models;
using Newtonsoft.Json;
using ChemColud.Shipping;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class AnalysisController : BaseMemberController
    {
        // GET: Web/Analysis
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(long Id)
        {
            ProductAnalysis model = ServiceHelper.Create<IProductAnalysisService>().GetProductAnalysis(Id);
            if (model != null)
            {
                model.MemberName = ServiceHelper.Create<IMemberService>().GetMember(model.Memberid) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(model.Memberid).UserName;
            }
            return View(model);
        }

        /*列表*/
        [HttpPost]
        [UnAuthorize]
        public JsonResult QueryList(int page, int rows, string ClientName, string SampleName)
        {
            ProductAnalysis querymodel = new ProductAnalysis()
            {
                PageSize = rows,
                PageNo = page,
                ClientName = ClientName,
                Memberid = CurrentUser.Id,
                SampleName = SampleName
            };

            PageModel<ProductAnalysis> source = ServiceHelper.Create<IProductAnalysisService>().GetProductAnalysisList(querymodel);
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

        /*删除*/
        [HttpPost]
        public JsonResult Delete(long Id)
        {
            bool flag = ServiceHelper.Create<IProductAnalysisService>().Delete(Id);
            Result result = new Result()
            {
                success = flag
            };
            return Json(result);
        }
    }
}