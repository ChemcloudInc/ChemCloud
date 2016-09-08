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
using System.IO;
using System.Text;
using ChemCloud.Core.Helper;
using ChemCloud.QueryModel;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class MessageDetailController : BaseWebController
    {
        // GET: Web/MessageDetail
        public ActionResult Management()
        {
            return View();
        }

        public JsonResult list(int page, int rows, int ReadFlag, string BeginTime = "", string EndTime = "")
        {
            MessageReviceQuery opQuery = new MessageReviceQuery();
            DateTime dt;
            if (DateTime.TryParse(BeginTime, out dt) && DateTime.TryParse(EndTime, out dt))
            {
                opQuery = new MessageReviceQuery()
                {
                    BeginTime = DateTime.Parse(BeginTime),
                    EndTime = DateTime.Parse(EndTime),
                    ReadFlag = ReadFlag,
                    UserId = base.CurrentUser.Id,
                    Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    PageNo = page,
                    PageSize = rows
                };
            }
            else
            {
                opQuery = new MessageReviceQuery()
                {
                    UserId = base.CurrentUser.Id,
                    Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    ReadFlag = ReadFlag,
                    PageNo = page,
                    PageSize = rows
                };
            }
            PageModel<MessageRevice> opModel = ServiceHelper.Create<IMessageDetialService>().GetMessageDetialByUserId(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, SendTime = item.SendTime, MsgId = item.MsgId, MsgType = item.MessageDetial.MsgType, SendObj = item.MessageDetial.SendObj, MessageContent = item.MessageDetial.MessageContent, MessageTitle = item.MessageDetial.MessageTitle, Status = item.ReadFlag };
            return Json(new { rows = array, total = opModel.Total });
        }

        public ActionResult Detail(long Id)
        {
            MessageRevice mr = ServiceHelper.Create<IMessageDetialService>().GetMessageById(Id);
            //MessageEnclosure me = ServiceHelper.Create<IMessageDetialService>().GetMessageEnclosureById(Id);
            //ViewBag.url = me.Url;
            if (mr.ReadFlag==1)
            { 
                ServiceHelper.Create<IMessageDetialService>().UpdateReadState(Id);
                mr.ReadFlag = 2;
                mr.ReadTime = DateTime.Now;
            }
           
            return View(mr);
        }


        public JsonResult Delete(long id)
        {
            bool flag = ServiceHelper.Create<IMessageDetialService>().DeleteMessageDetial(id);
            if (flag)
                return Json(new { success = true, msg = "删除成功！" });
            else
                return Json(new { success = false, msg = "删除失败！" });
        }
        public JsonResult BatchDelete(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            ServiceHelper.Create<IMessageDetialService>().BatchDeleteMessageRevice(nums.ToArray());
            Result result = new Result()
            {
                success = true,
                msg = "批量删除成功！"
            };
            return Json(result);
        }

        public JsonResult UpdateType(long id)
        {
            ServiceHelper.Create<IMessageDetialService>().UpdateReadState(id);
            Result result = new Result()
            {
                success = true,
                msg = "标记已读成功！"
            };
            return Json(result);
        }
    }
}