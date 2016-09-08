using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class TechInfoController : BaseAdminController
    {
        // GET: Admin/TechInfo
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult Auditing(long Id)
        {
            TechnicalInfo model = ServiceHelper.Create<ITechnicalInfoService>().GetTechInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<ITechnicalInfoService>().GetAttachmentInfosById(Id);
            ViewBag.parentId = Id;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View(model);
        }
        public ActionResult Detail(long Id)
        {
            TechnicalInfo model = ServiceHelper.Create<ITechnicalInfoService>().GetTechInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<ITechnicalInfoService>().GetAttachmentInfosById(Id);
            ViewBag.parentId = Id;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View(model);
        }
        [HttpPost]
        public JsonResult List(int page, int rows ,int? status, string title, string BeginTime = "", string EndTime = "")
        {
            TechnicalInfoQuery model = new TechnicalInfoQuery();
            model.Title = title;
            model.Status = status;
            DateTime dt;
            if (DateTime.TryParse(BeginTime, out dt) && DateTime.TryParse(EndTime, out dt))
            {
                model.BeginTime = DateTime.Parse(BeginTime);
                model.EndTime = DateTime.Parse(EndTime);
                model.PageNo = page;
                model.PageSize = rows;
            }
            else
            {
                model.PageNo = page;
                model.PageSize = rows;
            }
            PageModel<TechnicalInfo> opModel = ServiceHelper.Create<ITechnicalInfoService>().GetTechInfos(model,0);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, Title = item.Title, PublishTime = item.PublishTime, Status = item.Status};
            return Json(new { rows = array, total = opModel.Total });
        }
        [HttpPost]
        public JsonResult UpdateTechinfoStatus(long Id, int status)
        {
            bool flag = ServiceHelper.Create<ITechnicalInfoService>().UpdateTechinfoStatus(Id, status, base.CurrentManager.Id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult Delete(long id)
        {
            bool flag = ServiceHelper.Create<ITechnicalInfoService>().DeleteTechInfo(id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult BatchDelete(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            bool flag = ServiceHelper.Create<ITechnicalInfoService>().BatchDelete(nums.ToArray());
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
    }
}