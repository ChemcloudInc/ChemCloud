using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Admin.Models.Product;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class AnalysisController : BaseAdminController
    {
        // GET: Admin/Analysis
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
        public JsonResult QueryList(int page, int rows, string ClientName, string SampleName, string AnalysisStatus)
        {
            ProductAnalysis querymodel = new ProductAnalysis()
            {
                PageSize = rows,
                PageNo = page,
                ClientName = ClientName,
                Memberid = 0,
                SampleName = SampleName,
                AnalysisStatus = int.Parse(AnalysisStatus)
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

        /*更新状态*/
        [HttpPost]
        public JsonResult UpdateAnalysisStatus(long Id, int AnalysisStatus)
        {
            bool flag = ServiceHelper.Create<IProductAnalysisService>().UpdateAnalysisStatus(Id, AnalysisStatus);
            Result result = new Result()
            {
                success = flag
            };
            return Json(result);
        }

        /*上传*/
        public ActionResult UploadFile(long Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public JsonResult AddAttachment(long Id, string attachmentName)
        {
            bool flag = ServiceHelper.Create<IProductAnalysisService>().UpdateAnalysisAddAttachment(Id, attachmentName);
            Result result = new Result()
            {
                success = flag
            };
            return Json(result);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile()
        {
            string str = "";
            string filename = "";
            if (base.Request.Files.Count == 0)
            {
                return base.Content("NoFile", "text/html");
            }
            HttpPostedFileBase item = base.Request.Files[0];
            if (item == null || item.ContentLength <= 0 || !IsAllowExt(item))
            {
                return base.Content("The format is not correct！", "text/html");
            }
            if (item != null)
            {
                string extension = System.IO.Path.GetExtension(item.FileName);
                string serverFilepath = Server.MapPath("/Storage/FileTemplate");
                string serverFileName = System.IO.Path.GetFileNameWithoutExtension(item.FileName);
                serverFileName = serverFileName + DateTime.Now.ToString("yyyyMMddHHmmssffff") + extension;
                if (!System.IO.Directory.Exists(serverFilepath))
                {
                    System.IO.Directory.CreateDirectory(serverFilepath);
                }
                string File = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Storage\\FileTemplate\\", serverFileName);
                filename = File;
                try
                {
                    object obj = Cache.Get("Cache-UserUploadFile");
                    if (obj != null)
                    {
                        Cache.Insert("Cache-UserUploadFile", int.Parse(obj.ToString()) + 1);
                    }
                    else
                    {
                        Cache.Insert("Cache-UserUploadFile", 1);
                    }
                    item.SaveAs(File);
                    str = ImgHelper.PostImg(File, "file");//文件路径
                }
                catch
                {

                }

            }
            return base.Content(str, "text/html", System.Text.Encoding.UTF8);//将文件名回传给页面的innerHTML
        }

        private bool IsAllowExt(HttpPostedFileBase item)
        {
            var notAllowExt = "," + ConfigurationManager.AppSettings["NotAllowExt"] + ",";
            var ext = "," + Path.GetExtension(item.FileName).ToLower() + ",";
            if (notAllowExt.Contains(ext))
                return false;
            return true;
        }
    }
}