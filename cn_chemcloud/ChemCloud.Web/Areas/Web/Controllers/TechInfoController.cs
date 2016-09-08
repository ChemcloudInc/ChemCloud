using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Model.Common;
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
using System.Web.Services;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class TechInfoController : BaseWebController
    {
        public ActionResult Management()
        {
            return View();
        }
        public ActionResult ManagementWeb()
        {
            return View();
        }
        public ActionResult ManagementWebDes()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Uploadfile(long parentId, string attachmentName, string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<ITechnicalInfoService>().GetAttachmentInfosById(parentId);
            ViewBag.parentId = parentId;
            ViewBag.type = type;
            ViewBag.attachmentName = attachmentName;
            ViewBag.attachmentInfo = models;
            ViewBag.attachmentCount = models.Count;
            return View();
        }
        public ActionResult EditUploadfile(long parentId, string attachmentName, string type)
        {
            List<AttachmentInfo> models = ServiceHelper.Create<ITechnicalInfoService>().GetAttachmentInfosById(parentId);
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
            TechnicalInfo model = ServiceHelper.Create<ITechnicalInfoService>().GetTechInfo(Id);
            List<AttachmentInfo> models = ServiceHelper.Create<ITechnicalInfoService>().GetAttachmentInfosById(Id);
            ViewBag.attachmentCount = models.Count;
            ViewBag.LanguageType = model.LanguageType;
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
        public JsonResult AddTechnicalInfo(TechnicalInfoAddQuery json)
        {
            TechnicalInfo techInfo = new TechnicalInfo();
            techInfo.Title = json.Title;
            techInfo.TechContent = json.TechContent;
            techInfo.Status = json.Status;
            techInfo.Author = json.Author;
            techInfo.Tel = json.Tel;
            techInfo.Email = json.Email;
            techInfo.PublisherId = base.CurrentUser.Id;
            techInfo.LanguageType = json.LanguageType;
            TechnicalInfo models = ServiceHelper.Create<ITechnicalInfoService>().AddTechnicalInfo(techInfo);
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
            attachmentInfo.UserId = base.CurrentUser.Id;
            attachmentInfo.Type = 3;
            bool flag = ServiceHelper.Create<ITechnicalInfoService>().AddAttachment(attachmentInfo);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateTechInfo(TechnicalInfoModifyQuery json)
        {
            bool flag = false;
            TechnicalInfo model = ServiceHelper.Create<ITechnicalInfoService>().GetTechInfo(json.Id);
            if (model != null)
            {
                model.Title = json.Title;
                model.TechContent = json.TechContent;
                model.Author = json.Author;
                model.Tel = json.Tel;
                model.Email = json.Email;
                model.PublisherId = base.CurrentUser.Id;
                model.LanguageType = json.LanguageType;
                flag = ServiceHelper.Create<ITechnicalInfoService>().UpdateTechInfo(model);
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
            //object obj = Cache.Get("Cache-UserUploadFile");
            //if (obj != null)
            //{
            //    num = (string.IsNullOrEmpty(obj.ToString()) ? 0 : long.Parse(obj.ToString()));
            //}
            return Json(new { Count = num }, JsonRequestBehavior.AllowGet);
        }
        [WebMethod]
        public JsonResult UpdateAttachment(long attachmentId, string attachmentName)
        {
            Result_Msg res = new Result_Msg();
            AttachmentInfo attInfo = ServiceHelper.Create<ITechnicalInfoService>().GetAttachmentInfoById(attachmentId);
            if (attInfo != null)
            {
                attInfo.AttachmentName = attachmentName;
                attInfo.UserId = base.CurrentUser.Id;
                res = ServiceHelper.Create<ITechnicalInfoService>().UpdateAttachment(attInfo);
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "记录不存在";
            }
            return Json(res);
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
        [HttpPost]
        public JsonResult DeleteAttachment(long Id)
        {
            bool flag = ServiceHelper.Create<ITechnicalInfoService>().DeleteAttachment(Id);
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

        [HttpPost]
        public JsonResult List(int page, int rows, int? status, string title, string BeginTime = "", string EndTime = "")
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
            PageModel<TechnicalInfo> opModel = ServiceHelper.Create<ITechnicalInfoService>().GetTechInfos(model, base.CurrentUser.Id);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, Title = item.Title, PublishTime = item.PublishTime, Status = item.Status };
            return Json(new { rows = array, total = opModel.Total });
        }

        #region 前台技术资料

        #region 根据ID获取指定对象
        /// <summary>
        /// 根据ID获取指定对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetObjectById_Web(int Id)
        {

            Result_Model_TechAttachmentList_PreNextRow resAll = new Result_Model_TechAttachmentList_PreNextRow()
            {
                Model = new Result_TechnicalInfo(),
                AttachmentList = new Result_List<Result_AttachmentInfo>(),
                Msg = new Result_Msg() { IsSuccess = true, Message = string.Empty },
                PreNextRow = new Result_List<Result_Model<TechnicalInfo>>()
            };
            try
            {
                ITechnicalInfoService jobsService = ServiceHelper.Create<ITechnicalInfoService>();
                resAll.Model = jobsService.GetObjectById_Web(Id);
                if (resAll.Model != null && resAll.Msg.IsSuccess)
                {
                    resAll.AttachmentList = GetObjectList_ById_Web(Id);
                    if (!resAll.AttachmentList.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "Failed to obtain a list of attachments \n\r";
                    }
                    else
                    {
                        foreach (var item in resAll.AttachmentList.List)
                        {
                            try
                            {
                                item.FileName = item.AttachmentName.Substring(item.AttachmentName.LastIndexOf('/') + 1, item.AttachmentName.Length - item.AttachmentName.LastIndexOf('/') - 1);
                            }
                            catch (Exception)
                            {
                                item.FileName = "附件信息获取失败";
                            }
                        }
                    }
                    resAll.PreNextRow = Get_PreNext_ById_Web(Id);
                    if (!resAll.PreNextRow.Msg.IsSuccess)
                    {
                        resAll.Msg.IsSuccess = false;
                        resAll.Msg.Message += "Get the last page, the next page \n\r";
                    }
                }
                else
                {
                    resAll.Msg.IsSuccess = false;
                    resAll.Msg.Message += "Failed to get conference data \n\r";
                }
            }
            catch (Exception ex)
            {
                resAll.Msg = new Result_Msg() { IsSuccess = false, Message = "Failed to read, cause of failure ：" + ex.Message };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(resAll);
        }
        #endregion

        #region 会议中心  列表
        /// <summary>
        /// 会议中心  列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [WebMethod]
        public string MeetingsList_Web(QueryCommon<TechnicalInfoQuery_Web> query)
        {
            if (query.ParamInfo.Type > 0)
            {
                query.ParamInfo.EndTime = DateTime.Now.Date;
            }
            //else
            //{
            //    query.ParamInfo.StartTime = query.ParamInfo.StartTime.Date;
            //    query.ParamInfo.EndTime = query.ParamInfo.EndTime.Date.AddDays(1);
            //}

            ITechnicalInfoService jobsService = ServiceHelper.Create<ITechnicalInfoService>();
            Result_List_Pager<Result_TechnicalInfo> res = jobsService.GetMeetingsList_Web(query);
            List<Result_AttachmentInfo> resItem = new List<Result_AttachmentInfo>();
            foreach (var item in res.List)
            {
                resItem = GetObjectList_ById_Web(item.Id).List.Where(x => x.AttachmentName != null && x.AttachmentName != "" && x.AttachmentName.LastIndexOf('.') > 0).ToList();
                if (resItem != null && resItem.Count > 0)
                {
                    Result_AttachmentInfo imgItem = resItem.Where(x => HashSet_Common.ImageTypeArr.Contains(x.AttachmentName.Substring(x.AttachmentName.LastIndexOf('.'), x.AttachmentName.Length - x.AttachmentName.LastIndexOf('.')))).FirstOrDefault();
                    if (imgItem != null)
                    {
                        item.AttachmentName = imgItem.AttachmentName;

                    }
                    else
                    {
                        item.AttachmentName = "";
                    }
                }
                else
                {
                    item.AttachmentName = "";
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        #endregion

        #region 根据ID获取附件列表
        /// <summary>
        /// 根据ID获取附件列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id)
        {
            Result_List<Result_AttachmentInfo> res = new Result_List<Result_AttachmentInfo>();

            try
            {
                ITechnicalInfoService jobsService = ServiceHelper.Create<ITechnicalInfoService>();
                res = jobsService.GetObjectList_ById_Web(Id);
                //res.List.Select(x => x.FileName == System.IO.Path.GetFileName(x.AttachmentName));
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "Failed to read, cause of failure ：" + ex.Message };
            }

            return res;
        }
        #endregion

        #region 根据ID 前后两条信息
        /// <summary>
        /// 根据ID 前后两条信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result_List<Result_Model<TechnicalInfo>> Get_PreNext_ById_Web(long Id)
        {
            Result_List<Result_Model<TechnicalInfo>> res = new Result_List<Result_Model<TechnicalInfo>>();

            try
            {
                ITechnicalInfoService jobsService = ServiceHelper.Create<ITechnicalInfoService>();
                res = jobsService.Get_PreNext_ById_Web(Id);
            }
            catch (Exception ex)
            {
                res.Msg = new Result_Msg() { IsSuccess = false, Message = "Failed to read, cause of failure ：" + ex.Message };
            }

            return res;
        }
        #endregion

        #region 获取分页信息
        [WebMethod]

        public string Get_PageInfo(QueryCommon<TechnicalInfoQuery_Web> query)
        {
            if (query.ParamInfo.Type > 0)
            {
                query.ParamInfo.EndTime = DateTime.Now.Date;
            }
            //else
            //{
            //    query.ParamInfo.StartTime = query.ParamInfo.StartTime.Date;
            //    query.ParamInfo.EndTime = query.ParamInfo.EndTime.Date.AddDays(1);
            //}

            Result_Model<PageInfo> resModel = new Result_Model<PageInfo>();
            ITechnicalInfoService jobsService = ServiceHelper.Create<ITechnicalInfoService>();
            resModel = jobsService.Get_PageInfo_Web(query);
            return Newtonsoft.Json.JsonConvert.SerializeObject(resModel);
        }
        #endregion
        #endregion
    }
}