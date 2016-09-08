using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class MeetingInfoController : BaseAdminController
    {
        // GET: Admin/MeetingInfo
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Uploadfile(long parentId,string attachmentName,string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<IMeetInfoService>().GetAttachmentInfosById(parentId);
            ViewBag.parentId = parentId;
            ViewBag.type = type;
            ViewBag.attachmentName = attachmentName;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View();
        }
        public ActionResult EditUploadfile(long parentId, string attachmentName, string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<IMeetInfoService>().GetAttachmentInfosById(parentId);
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
            MeetingInfo model = ServiceHelper.Create<IMeetInfoService>().GetMeetInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<IMeetInfoService>().GetAttachmentInfosById(Id);
            ViewBag.attachmentCount = models.Count;
            ViewBag.LanguageType = model.LanguageType;
            return View(model);
        }
        public ActionResult Detail(long Id)
        {
            MeetingInfo model = ServiceHelper.Create<IMeetInfoService>().GetMeetInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<IMeetInfoService>().GetAttachmentInfosById(Id);
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            ViewBag.parentId = Id;
            return View(model);
        }
        //[HttpPost]
        [WebMethod]
        public JsonResult AddMeetingInfo(MeetingInfoAddQuery json)
        {
            MeetingInfo meetInfo = new MeetingInfo();
            meetInfo.Title = json.Title;
            meetInfo.MeetingTime = json.MeetingTime;
            meetInfo.MeetingPlace = json.MeetingPlace;
            meetInfo.MeetingContent = json.MeetingContent;
            meetInfo.ContinueTime = json.ContinueTime;
            meetInfo.UserId = base.CurrentManager.Id;
            meetInfo.LanguageType = json.LanguageType;
            MeetingInfo models = ServiceHelper.Create<IMeetInfoService>().AddMeetInfo(meetInfo);
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
            attachmentInfo.Type = 1;
            bool flag = ServiceHelper.Create<IMeetInfoService>().AddAttachment(attachmentInfo);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateMeetingInfo(MeetingInfoModifyQuery json)
        {
            bool flag = false;
            MeetingInfo model = ServiceHelper.Create<IMeetInfoService>().GetMeetInfo(json.Id);
            if(model != null)
            {
                model.Title = json.Title;
                model.MeetingTime = json.MeetingTime;
                model.MeetingPlace = json.MeetingPlace;
                model.MeetingContent = json.MeetingContent;
                model.CreatDate = DateTime.Now;
                model.ContinueTime = json.ContinueTime;
                model.UserId = base.CurrentManager.Id;
                model.LanguageType = json.LanguageType;
                flag = ServiceHelper.Create<IMeetInfoService>().UpdateMeetInfo(model);
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
            AttachmentInfo attInfo = ServiceHelper.Create<IMeetInfoService>().GetAttachmentInfoById(attachmentId);
            if (attInfo != null)
            {
                attInfo.AttachmentName =  attachmentName;
                attInfo.UserId = base.CurrentManager.Id;
                flag = ServiceHelper.Create<IMeetInfoService>().UpdateAttachment(attInfo);
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
            bool flag = ServiceHelper.Create<IMeetInfoService>().DeleteMeetingInfo(id);
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
            bool flag = ServiceHelper.Create<IMeetInfoService>().BatchDelete(nums.ToArray());
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult DeleteAttachment(long Id)
        {
            bool flag = ServiceHelper.Create<IMeetInfoService>().DeleteAttachment(Id);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
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
        [HttpPost]
        public JsonResult List(int page, int rows, string BeginTime = "", string EndTime = "")
        {
            MeetingInfoQuery model = new MeetingInfoQuery();
            DateTime dt;
            if (DateTime.TryParse(BeginTime, out dt) && DateTime.TryParse(EndTime, out dt))
            {
                model = new MeetingInfoQuery()
                {
                    BeginTime = DateTime.Parse(BeginTime),
                    EndTime = DateTime.Parse(EndTime),
                    PageNo = page,
                    PageSize = rows
                };
            }
            else
            {
                model = new MeetingInfoQuery()
                {
                    PageNo = page,
                    PageSize = rows
                };
            }
            PageModel<MeetingInfo> opModel = ServiceHelper.Create<IMeetInfoService>().GetMeetingInfos(model);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, Title = item.Title, MeetingTime = item.MeetingTime, MeetingPlace = item.MeetingPlace, MeetingContent = item.MeetingContent};
            return Json(new { rows = array, total = opModel.Total });
        }
    }
}