using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Web.Models;
using ChemCloud.Web.Areas.Admin.Models;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class MessageController : BaseMemberController
    {
        // GET: Web/Message
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Management()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string Status, int page, int rows)
        {

            SiteMessagesQuery siteMessagesQuery = new SiteMessagesQuery()
            {
                PageSize = rows,
                PageNo = page,
                MemberId = base.CurrentUser.Id,
            };
            SiteMessagesQuery siteMessagesQuery1 = siteMessagesQuery;


            switch (Status)
            {
                case "0": siteMessagesQuery1.Status = null; break;
                case "1": siteMessagesQuery1.Status = SiteMessages.Status.UnRead; break;
                case "2": siteMessagesQuery1.Status = SiteMessages.Status.Readed; break;
            }


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
            SiteMessages model = ServiceHelper.Create<ISiteMessagesService>().GetMessage(Id);
            model.ReceiveName = ServiceHelper.Create<IMemberService>().GetMember(long.Parse(model.MemberId.ToString())) == null ? "" : ServiceHelper.Create<IMemberService>().GetMember(long.Parse(model.MemberId.ToString())).UserName;

            model.SendName = model.SendName;

            if (model != null)
            {
                UpdateType(Id);
            }
            return View(model);
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
        [HttpPost]
        [ShopOperationLog(Message = "消息标为已读")]
        public JsonResult UpdateType(long Id)
        {
            Result result = new Result();
            try
            {
                SiteMessages siteMessage = ServiceHelper.Create<ISiteMessagesService>().GetMessage(Id);
                siteMessage.ReadStatus = SiteMessages.Status.Readed;

                ServiceHelper.Create<ISiteMessagesService>().UpdateMessage(siteMessage);

                result.success = true;
            }
            catch (Exception exception)
            {
                result.msg = exception.Message;
            }
            return Json(result);
        }
    }
}