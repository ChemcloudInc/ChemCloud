using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using O2S.Components.PDFRender4NET;
using iTextSharp.text;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class MessageDetialController : BaseAdminController
    {
        // GET: Admin/MessageDetial
        public ActionResult Management()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List(int page, int rows, int MsgType, int SendOBj, string BeginTime = "", string EndTime = "")
        {

            MessageDetialQuery opQuery = new MessageDetialQuery();
            DateTime dt;
            if (DateTime.TryParse(BeginTime, out dt) && DateTime.TryParse(EndTime, out dt))
            {
                opQuery = new MessageDetialQuery()
                {
                    BeginTime = DateTime.Parse(BeginTime),
                    EndTime = DateTime.Parse(EndTime),
                    SendObj = SendOBj,
                    MsgType = 1,
                    PageNo = page,
                    PageSize = rows
                };
            }
            else
            {
                opQuery = new MessageDetialQuery()
                {
                    SendObj = SendOBj,
                    MsgType = 1,
                    PageNo = page,
                    PageSize = rows
                };
            }
            PageModel<MessageDetial> opModel = ServiceHelper.Create<IMessageDetialService>().SelectAllMessageDetial(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, SendTime = item.SendTime, MsgType = item.MsgType, SendObj = item.SendObj, MessageContent = item.MessageContent, MessageTitle = item.MessageTitle };
            return Json(new { rows = array, total = opModel.Total });
        }

        public JsonResult Delete(long id)
        {
            bool flag = ServiceHelper.Create<IMessageDetialService>().DeleteMessageDetial(id);
            if (flag)
                return Json(new { success = true, msg = "删除成功！" });
            else
                return Json(new { success = false, msg = "删除失败！" });
        }



        public JsonResult GetMembers(int receType)
        {
            List<long> memberIds = ServiceHelper.Create<ISiteMessagesService>().GetMemberIds(receType);
            if (memberIds.Count > 0)
                return Json(new { success = true, data = memberIds });
            else
                return Json(new { success = false, msg = "获取消息接收人失败" });
        }
        public JsonResult GetManagers(string Search)
        {
            List<UserMemberInfo> list = ServiceHelper.Create<IManagerService>().GetMenberIdByName(Search);
            return Json(new { success = true, data = list });
        }
        public JsonResult GetCManagers(int typeid)
        {
            List<UserMemberInfo> list = ServiceHelper.Create<IManagerService>().GetMenberIdByShopState(typeid);
            return Json(new { success = true, data = list });
        }
        public JsonResult GetLanguage()
        {
            List<ChemCloud_Dictionaries> dicts = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(10).ToList();
            List<QueryMember> managerInfos = ServiceHelper.Create<IMessageDetialService>().GetLanguage(dicts);
            if (managerInfos != null)
                return Json(new { success = true, data = managerInfos });
            else
                return Json(new { success = false });
        }
        public JsonResult GetMangers(int receType, string Search)
        {
            IQueryable<QueryMember> managerInfos = ServiceHelper.Create<ISiteMessagesService>().GetMangers(receType, Search);
            if (managerInfos != null)
                return Json(new { success = true, data = managerInfos });
            else
                return Json(new { success = false });
        }
        //receType: value, messageContent: $("#MessageContent").html(), ids: IdArray  int receType, int messageTitleId,string messageTitle, string MessageContent,int LanguageType, string[] ids
        public JsonResult SendMessage(MessageDetialAddQuery model)
        {
            MessageDetial md = new MessageDetial();
            md.ManagerId = base.CurrentManager.Id;
            md.MessageTitleId = model.MessageTitleId;
            md.MessageTitle = model.MessageTitle;
            md.MessageContent = model.MessageContent;
            md.LanguageType = model.LanguageType;
            md.MsgType = 1;
            md.SendTime = DateTime.Now;
            md.SendObj = model.ReceType;
            ServiceHelper.Create<IMessageDetialService>().AddMessageDetial(md, model.Ids, null);
            Result res = new Result();
            res.success = true;
            return Json(res);
        }


        public JsonResult GetReadState(long id)
        {
            return Json(ServiceHelper.Create<IMessageDetialService>().GetReadState(id));
        }

        public ActionResult AddMessage()
        {
            ViewBag.LanDics = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(10);
            return View();
        }
        [HttpPost]
        public JsonResult UploadEnclosure()
        {
            Result result = new Result();
            string str = "NoEnclosure";
            HttpPostedFileBase item = null;
            if (base.Request.Files.Count > 0)
            {
                var objfile = base.Request.Files[0];
                if (objfile != null)
                {
                    item = objfile;
                }
                //HttpPostedFileBase item = base.Request.Files[0];
                if (item != null)
                {
                    string serverFilepath = "";
                    string serverFileName = "";
                    string fileExtension = System.IO.Path.GetExtension(item.FileName).ToLower();
                    if (fileExtension == ".xls")
                    {
                        serverFilepath = Server.MapPath(string.Format("/Storage/Enclosure/{0}/Excel ", base.CurrentManager.Id));
                        serverFileName = "Enclosure" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".XLS";
                    }
                    else if (fileExtension == ".doc")
                    {
                        serverFilepath = Server.MapPath(string.Format("/Storage/Enclosure/{0}/Word ", base.CurrentManager.Id));
                        serverFileName = "Enclosure" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".DOC";
                    }
                    else if (fileExtension == ".pdf")
                    {
                        serverFilepath = Server.MapPath(string.Format("/Storage/Enclosure/{0}/Pdf ", base.CurrentManager.Id));
                        serverFileName = "Enclosure" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".PDF";
                    }
                    else if (fileExtension == ".jpg")
                    {
                        serverFilepath = Server.MapPath(string.Format("/Storage/Enclosure/{0}/jpg ", base.CurrentManager.Id));
                        serverFileName = "Enclosure" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".JPG";
                    }
                    string File = serverFilepath + '\\' + serverFileName;
                    if (!System.IO.Directory.Exists(serverFilepath))
                    {
                        System.IO.Directory.CreateDirectory(serverFilepath);
                    }
                    try
                    {
                        item.SaveAs(File);
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Log.Error(string.Concat("产品导入上传文件异常：", exception.Message));
                        result.msg = "Error";
                        result.success = false;
                    }
                    result.msg = File;
                    result.success = true;
                }
                else
                {
                    result.msg = "文件长度为0,格式异常。";
                    result.success = false;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetMessageContent(int TitleId)
        {
            string content = "";
            MessageSetting messageInfo = ServiceHelper.Create<IMessageDetialService>().GetMessageContent(TitleId);
            if (messageInfo != null)
            {
                content = messageInfo.MessageContent;
                return Json(new { success = true, data = content });
            }
            else
            {
                return Json(new { success = false });
            }
        }


      
    }
}