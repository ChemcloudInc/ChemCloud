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
using System.Web.Script;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    public class MessageController : BaseSellerController
    {
        // GET: SellerAdmin/Message
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Management()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List(long? Id, long? MemberId, int? Status, int? ReceStatus, int? MessageModule, int page, int rows)
        {
            SiteMessages.Status? readStatus;
            SiteMessages.ReceiveType? receStatus;
            MessageSetting.MessageModuleStatus? messageModule;
            SiteMessagesQuery siteMessagesQuery = new SiteMessagesQuery()
            {
                PageSize = rows,
                PageNo = page,
                Id = Id,
                MemberId = base.CurrentUser.Id
            };
            SiteMessagesQuery siteMessagesQuery1 = siteMessagesQuery;
            int? readStatus1 = Status;
            if (readStatus1.HasValue && readStatus1 != 0)
            {
                readStatus = new SiteMessages.Status?((SiteMessages.Status)readStatus1.GetValueOrDefault());
            }
            else
            {
                readStatus = null;
            }
            siteMessagesQuery1.Status = readStatus;
            //int? receStatus1 = 2;
            //if (receStatus1.HasValue)
            //{
            //    receStatus = new SiteMessages.ReceiveType?((SiteMessages.ReceiveType)receStatus1.GetValueOrDefault());
            //}
            //else
            //{
            //    receStatus = null;
            //}
            //siteMessagesQuery.ReceStatus = receStatus;
            int? messageModule1 = MessageModule;
            if (messageModule1.HasValue && messageModule1 != 0)
            {
                messageModule = new MessageSetting.MessageModuleStatus?((MessageSetting.MessageModuleStatus)MessageModule.GetValueOrDefault());
            }
            else
            {
                messageModule = null;
            }
            siteMessagesQuery1.MessageModule = messageModule;
            PageModel<SiteMessages> messageSetting = ServiceHelper.Create<ISiteMessagesService>().GetMessages(siteMessagesQuery1);
            IEnumerable<SiteMessagesModel> array =
                from item in messageSetting.Models.ToArray()
                select new SiteMessagesModel()
                {
                    Status = item.ReadStatus.ToDescription(),
                    ReceType = item.ReceType.ToDescription(),
                    MessageModule = item.MessageModule.ToDescription(),
                    MemberId = item.MemberId,
                    Id = item.Id,
                    ReceiveName = item.ReceiveName,
                    SendTime = item.SendTime,
                    ReadTime = item.ReadTime,
                    MessageContent = item.MessageContent,
                    SendName = item.SendName
                };
            DataGridModel<SiteMessagesModel> dataGridModel = new DataGridModel<SiteMessagesModel>()
            {
                rows = array,
                total = messageSetting.Total
            };
            return Json(dataGridModel);
        }
        public ActionResult Detail(long Id)
        {
            SiteMessages siteMessage = ServiceHelper.Create<ISiteMessagesService>().GetMessage(Id);
            if (siteMessage.ReadStatus == SiteMessages.Status.UnRead)
            {
                ServiceHelper.Create<ISiteMessagesService>().UpdateMessage(siteMessage);
            }
            SiteMessagesModel siteMessageModel = new SiteMessagesModel(siteMessage);
            return View(siteMessageModel);
        }
        [HttpPost]
        [ShopOperationLog(Message = "消息标为已读")]
        public JsonResult UpdateType(long Id)
        {
            Result result = new Result();
            try
            {
                ISiteMessagesService MessageService = ServiceHelper.Create<ISiteMessagesService>();
                SiteMessages siteMessage = ServiceHelper.Create<ISiteMessagesService>().GetMessage(Id);
                MessageService.UpdateMessage(siteMessage);
                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }
        [HttpPost]

        public JsonResult Delete(long Id)
        {
            bool flag = ServiceHelper.Create<ISiteMessagesService>().DeleteMessage(Id);
            Result result = new Result()
            {
                success = true,
                msg = "删除成功！"
            };
            return Json(result);
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
            ServiceHelper.Create<ISiteMessagesService>().BatchDeleteMessages(nums.ToArray());
            Result result = new Result()
            {
                success = true,
                msg = "批量删除成功！"
            };
            return Json(result);
        }
    }
}