using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class LawInfoController : BaseAdminController
    {
        // GET: SellerAdmin/LawInfo
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Uploadfile(long parentId, string attachmentName, string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<ILawInfoService>().GetAttachmentInfosById(parentId);
            ViewBag.parentId = parentId;
            ViewBag.type = type;
            ViewBag.attachmentName = attachmentName;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View();
        }
        public ActionResult EditUploadfile(long parentId, string attachmentName, string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<ILawInfoService>().GetAttachmentInfosById(parentId);
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            ViewBag.parentId = parentId;
            ViewBag.type = type;
            if (models.Count > 0)
                return View();
            else
                return RedirectToAction("Management");
        }
        public ActionResult Edit(long Id)
        {
            LawInfo model = ServiceHelper.Create<ILawInfoService>().GetLawInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<ILawInfoService>().GetAttachmentInfosById(Id);
            ViewBag.attachmentCount = models.Count;
            ViewBag.LanguageType = model.LanguageType;
            return View(model);
        }
        public ActionResult Detail(long Id)
        {
            LawInfo model = ServiceHelper.Create<ILawInfoService>().GetLawInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<ILawInfoService>().GetAttachmentInfosById(Id);
            ViewBag.parentId = Id;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View(model);
        }
        [HttpPost]
        public JsonResult AddLawInfo(LawInfoAddQuery json)
        {
            LawInfo lawInfo = new LawInfo();
            lawInfo.Title = json.Title;
            lawInfo.LawsInfo = json.LawsInfo;
            lawInfo.Status = json.Status;
            lawInfo.Author = json.Author;
            lawInfo.UserId = base.CurrentManager.Id;
            lawInfo.LanguageType = json.LanguageType;
            LawInfo models = ServiceHelper.Create<ILawInfoService>().AddLawInfo(lawInfo);
            if (models != null)
                return Json(new { success = true, parentId = models.Id });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult AddAttachment(long parentId, string attachmentName)
        {
            AttachmentInfo attachmentInfo = new AttachmentInfo();
            attachmentInfo.ParentId = parentId;
            attachmentInfo.AttachmentName = attachmentName;
            attachmentInfo.UserId = base.CurrentManager.Id;
            attachmentInfo.Type = 2;
            bool flag = ServiceHelper.Create<ILawInfoService>().AddAttachment(attachmentInfo);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateLawInfo(LawInfoModifyQuery json)
        {
            bool flag = false;
            LawInfo model = ServiceHelper.Create<ILawInfoService>().GetLawInfo(json.Id);
            if (model != null)
            {
                model.Title = json.Title;
                model.LawsInfo = json.LawsInfo;
                model.Author = json.Author;
                model.UserId = base.CurrentManager.Id;
                model.LanguageType = json.LanguageType;
                flag = ServiceHelper.Create<ILawInfoService>().UpdateLawInfo(model);
            }
            else
            {
                flag = false;
            }
            if (flag)
                return Json(new { success = true, parentId = model.Id });
            else
                return Json(new { success = false });
        }
        public JsonResult GetImportOpCount()
        {
            long num = 0;
            object obj = Cache.Get("Cache-UserUploadFile");
            if (obj != null)
            {
                num = (string.IsNullOrEmpty(obj.ToString()) ? 0 : long.Parse(obj.ToString()));
            }
            return Json(new { Count = num }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateAttachment(long attachmentId, string attachmentName)
        {
            bool flag = false;
            AttachmentInfo attInfo = ServiceHelper.Create<ILawInfoService>().GetAttachmentInfoById(attachmentId);
            if (attInfo != null)
            {
                attInfo.AttachmentName = attachmentName;
                attInfo.UserId = base.CurrentManager.Id;
                flag = ServiceHelper.Create<ILawInfoService>().UpdateAttachment(attInfo);
            }
            else
            {
                flag = false;
            }
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult Delete(long id)
        {
            bool flag = ServiceHelper.Create<ILawInfoService>().DeleteLawInfo(id);
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
            bool flag = ServiceHelper.Create<ILawInfoService>().BatchDelete(nums.ToArray());
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult DeleteAttachment(long Id)
        {
            bool flag = ServiceHelper.Create<ILawInfoService>().DeleteAttachment(Id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult GetLanguage()
        {
            List<ChemCloud_Dictionaries> dicts = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(10).ToList();
            List<QueryMember> managerInfos = ServiceHelper.Create<IMessageDetialService>().GetLanguage(dicts);
            if (managerInfos != null)
                return Json(new { success = true, data = managerInfos });
            else
                return Json(new { success = false });
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
                return base.Content("格式不正确！", "text/html");
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
        [HttpPost]
        public JsonResult List(int page, int rows,  string title, string BeginTime = "", string EndTime = "")
        {
            LawInfoQuery model = new LawInfoQuery();
            model.Title = title;
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
            PageModel<LawInfo> opModel = ServiceHelper.Create<ILawInfoService>().GetLawInfos(model, 0);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, Title = item.Title, CreateTime = item.CreateTime, Status = item.Status };
            return Json(new { rows = array, total = opModel.Total });
        }
    }
}