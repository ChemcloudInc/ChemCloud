using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
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

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class MessageSettingController : BaseAdminController
    {

        public ActionResult Management(string type = "")
        {
            IEnumerable<SelectListItem> selectListItems = null;
            SelectList selectList = MessageSetting.MessageModuleStatus.OrderCreated.ToSelectList<MessageSetting.MessageModuleStatus>(true, false);
            dynamic viewBag = base.ViewBag;
            selectListItems = (
                from p in selectList
                select p);
            ViewBag.Status = selectListItems;
            List<SelectListItem> selectListItems1 = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Selected = true,
                Value = 0.ToString(),
                Text = "请选择..."
            };
            selectListItems1.Add(selectListItem);
            ViewBag.Type = type;
            return View();
        }
        [HttpPost]
        [UnAuthorize]
        public JsonResult List(long? Id, int? MessageNameId, int? Status, int page, int rows)
        {
            MessageSetting.MessageModuleStatus? nullable;
            MessageSettingQuery messageQuery = new MessageSettingQuery()
            {
                Id = Id,
                Status = Status
            };
            int? nullable1 = MessageNameId;
            if (nullable1.HasValue)
            {
                nullable = new MessageSetting.MessageModuleStatus?((MessageSetting.MessageModuleStatus)nullable1.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            MessageSettingQuery messageQuery1 = messageQuery;
            messageQuery.MessageNameId = nullable;
            messageQuery.PageSize = rows;
            messageQuery.PageNo = page;
            PageModel<MessageSetting> settings = ServiceHelper.Create<IMessageSettingService>().GetSettings(messageQuery);
            IMessageSettingService messageService = ServiceHelper.Create<IMessageSettingService>();
            IEnumerable<MessageSettingModel> array =
                from item in settings.Models.ToArray()
                select new MessageSettingModel()
                {
                    Id = item.Id,
                    MessageNameId = item.MessageNameId.ToDescription(),
                    MessageContent = item.MessageContent,
                    Languagetype = item.Languagetype,
                    LanguageName = item.LanguageName,
                    CreatDate = DateTime.Now,
                    ActiveStatus = item.ActiveStatus,
                    TitleId = (int)item.MessageNameId
                };
            DataGridModel<MessageSettingModel> dataGridModel = new DataGridModel<MessageSettingModel>()
            {
                rows = array,
                total = settings.Total
            };
            return Json(dataGridModel);
        }
        public ActionResult Detail(long Id)
        {
            MessageSetting messageSetting = ServiceHelper.Create<IMessageSettingService>().GetSetting(Id);
            MessageSettingModel MessageSettingModel = new MessageSettingModel(messageSetting);
            return View(MessageSettingModel);
        }
        [HttpPost]
        public JsonResult Adding(MessageSettingAddQuery MessageSettingInfo)
        {
            Result result1 = new Result();
            MessageSetting model = new MessageSetting();
            model.MessageNameId = (MessageSetting.MessageModuleStatus)MessageSettingInfo.MessageNameId;
            model.MessageContent = MessageSettingInfo.MessageContent;
            model.Languagetype = MessageSettingInfo.LanguageType;
            model.CreatDate = DateTime.Now;
            ServiceHelper.Create<IMessageSettingService>().AddModule(model);
            result1.success = true;
            return Json(result1);
        }
        [Description("新增消息配置页面")]
        [HttpGet]
        [UnAuthorize]
        public ActionResult Adding()
        {
            ViewBag.LanDics = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(10);
            return View();
        }
        [Description("编辑消息配置页面")]
        [HttpGet]
        public ActionResult Edit(long Id)
        {
            MessageSetting MessageSetting = ServiceHelper.Create<IMessageSettingService>().GetSetting(Id);
            MessageSettingModel MessageSettingModel = new MessageSettingModel(MessageSetting);
            ViewBag.Id = Id;
            return View(MessageSettingModel);
        }
        [HttpPost]
        public JsonResult Edit(MessageSettingModifyQuery MessageSettingInfo)//Id: Id, MessageContent: MessageContent, LanguageType: LanguageType, MessageNameId: MessageNameId
        {
            Result result1 = new Result();
            MessageSetting messageSettings = new MessageSetting();
            messageSettings.Id = MessageSettingInfo.Id;
            messageSettings.Languagetype = MessageSettingInfo.LanguageType;
            //messageSettings.MessageNameId = ((MessageSetting.MessageModuleStatus)(int.Parse(messageSettingModel.MessageNameId)));//messageSettingModel.MessageName;
            messageSettings.MessageContent = MessageSettingInfo.MessageContent;
            messageSettings.CreatDate = DateTime.Now;
            ServiceHelper.Create<IMessageSettingService>().UpdateMessageSetting(messageSettings);
            result1.success = true;
            return Json(result1);
        }
        [HttpPost]
        [UnAuthorize]
        public JsonResult Delete(long Id)
        {
            JsonResult jsonResult;
            try
            {
                ServiceHelper.Create<IMessageSettingService>().DeleteMessageSetting(Id);
                jsonResult = Json(new { success = true });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                jsonResult = Json(new { success = false, msg = exception.Message });
            }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ActiveMessageTemp(long Id, int titleId, int languageType)
        {
            bool flag = ServiceHelper.Create<IMessageSettingService>().ActiveStatus(Id,titleId,languageType);
            if(flag)
                return Json(new { success = true});
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UnActiveMessageTemp(long Id, int titleId, int languageType)
        {
            bool flag = ServiceHelper.Create<IMessageSettingService>().UnActiveStatus(Id, titleId, languageType);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult SetOtherUnActiveStatus(long Id, int titleId, int languageType)
        {
            ServiceHelper.Create<IMessageSettingService>().SetOtherUnActiveStatus(Id, titleId, languageType);
            return Json(new { success = true });
        }
    }
}