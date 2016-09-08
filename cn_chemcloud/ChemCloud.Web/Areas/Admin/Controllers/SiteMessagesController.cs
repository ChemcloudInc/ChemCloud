using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Areas.Admin.Models;
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
using System.Web.Script;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class SiteMessagesController : BaseAdminController
    {

        /// <summary>
        /// 获取页面下拉菜单
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
                    MsgType = 2,
                    PageNo = page,
                    PageSize = rows
                };
            }
            else
            {
                opQuery = new MessageDetialQuery()
                {
                    SendObj = SendOBj,
                    MsgType = 2,
                    PageNo = page,
                    PageSize = rows
                };
            }
            PageModel<MessageDetial> opModel = ServiceHelper.Create<IMessageDetialService>().SelectAllMessageDetial(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new
                {
                    Id = item.Id,
                    SendTime = item.SendTime,
                    MsgType = item.MsgType,
                    SendObj = item.SendObj,
                    MessageContent = item.MessageContent,
                    MessageTitle = item.MessageTitle,
                    ReadCount = ServiceHelper.Create<IMessageDetialService>().GetReadOrNoReadCount(item.Id, "read"),
                    NoreadCount = ServiceHelper.Create<IMessageDetialService>().GetReadOrNoReadCount(item.Id, "noread")
                };
            return Json(new { rows = array, total = opModel.Total });
        }
        [Description("新增消息页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult ModuleSendMessage()
        {
            ViewBag.LanDics = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(10);
            return View();
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
        [Description("新增消息配置页面(POST)")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult ModuleSendMessage(SiteMessageAddQuery SiteMessageInfo)
        {
            MessageDetial md = new MessageDetial();
            md.ManagerId = base.CurrentManager.Id;
            md.MessageTitleId = SiteMessageInfo.MessageTitleId;
            md.MessageContent = SiteMessageInfo.MessageContent;
            md.LanguageType = SiteMessageInfo.LanguageType;
            md.MsgType = 2;
            md.SendTime = DateTime.Now;
            md.SendObj = SiteMessageInfo.ReceType;
            ServiceHelper.Create<IMessageDetialService>().AddMessageDetial(md, SiteMessageInfo.Ids, null);
            Result res = new Result();
            res.success = true;
            return Json(res);
        }
        public ActionResult Detail(long Id)
        {
            MessageRevice mr = ServiceHelper.Create<IMessageDetialService>().GetMessageById(Id);
            return View(mr);
        }
        [Description("通过MessageModule获取(POST)")]
        [HttpPost]
        [UnAuthorize]
        [ValidateInput(false)]
        public JsonResult GetMessageByMessageModule(int messageModule)
        {
            Result result1 = new Result();
            result1.msg = ServiceHelper.Create<IMessageSettingService>().GetMessageByMessageModule(messageModule);
            if (!string.IsNullOrWhiteSpace(result1.msg))
            {
                result1.success = true;
            }
            else
            {
                result1.success = false;
            }
            return Json(result1);
        }
        [HttpPost]
        public JsonResult GetMangers(int receType, string Search)
        {
            IQueryable<QueryMember> managerInfos = ServiceHelper.Create<ISiteMessagesService>().GetMangers(receType, Search);
            if (managerInfos != null)
                return Json(new { success = true, data = managerInfos });
            else
                return Json(new { success = false });
        }
        [Description("获取所有接收人Id(POST)")]
        [HttpPost]
        [UnAuthorize]
        [ValidateInput(false)]
        public JsonResult GetMembers(int receType)
        {
            List<long> memberIds = ServiceHelper.Create<ISiteMessagesService>().GetMemberIds(receType);
            if (memberIds.Count > 0)
                return Json(new { success = true, data = memberIds });
            else
                return Json(new { success = false, msg = "获取消息接收人失败" });
        }
        public JsonResult GetReadState(long id)
        {
            return Json(ServiceHelper.Create<IMessageDetialService>().GetReadState(id));
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
        public JsonResult Delete(long id)
        {
            bool flag = ServiceHelper.Create<IMessageDetialService>().DeleteMessageDetial(id);
            if (flag)
                return Json(new { success = true, msg = "删除成功！" });
            else
                return Json(new { success = false, msg = "删除失败！" });
        }

    }
}