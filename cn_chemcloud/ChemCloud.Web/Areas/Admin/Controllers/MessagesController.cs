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

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class MessagesController : BaseAdminController
    {

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
                    UserId = base.CurrentManager.Id,
                    Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    PageNo = page,
                    PageSize = rows
                };
            }
            else
            {
                opQuery = new MessageReviceQuery()
                {
                    ReadFlag = ReadFlag,
                    UserId = base.CurrentManager.Id,
                    Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()),
                    PageNo = page,
                    PageSize = rows
                };
            }
            PageModel<MessageRevice> opModel = ServiceHelper.Create<IMessageDetialService>().GetMessageDetialByUserId(opQuery);
            var array =
                from item in opModel.Models.ToArray()
                select new { Id = item.Id, SendTime = item.SendTime, MsgType = item.MessageDetial.MsgType, SendObj = item.MessageDetial.SendObj, MessageContent = item.MessageDetial.MessageContent, MessageTitle = item.MessageDetial.MessageTitle };
            return Json(new { rows = array, total = opModel.Total });
        }

        public ActionResult Detial(long Id)
        {
            MessageRevice mr = ServiceHelper.Create<IMessageDetialService>().GetMessageById(Id);
            MessageEnclosure me = ServiceHelper.Create<IMessageDetialService>().GetMessageEnclosureById(Id);
            ViewBag.url = me.Url;
            return View(mr);
        }


        public JsonResult BatchDelete(int id)
        {
            ServiceHelper.Create<IMessageDetialService>().DeleteMessageRevice(id);
            Result res = new Result();
            res.success = true;
            res.msg = "删除成功";
            return Json(res);
        }

        public JsonResult Delete(string ids)
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