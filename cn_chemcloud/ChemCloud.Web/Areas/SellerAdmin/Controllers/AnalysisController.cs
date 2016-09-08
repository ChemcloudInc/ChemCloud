using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Express;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
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
    public class AnalysisController : BaseSellerController
    {
        // GET: SellerAdmin/Analysis
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