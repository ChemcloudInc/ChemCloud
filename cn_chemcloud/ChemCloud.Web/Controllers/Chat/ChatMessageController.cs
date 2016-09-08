//-------------------------------------------------------  
// <copyright  company='南京沃斯多克'> 
// Copyright 南京沃斯多克 All rights reserved.
// </copyright>  
//------------------------------------------------------- 
namespace ChemCloud.Web.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Razor;
    using System.Web.Routing;
    using System.Linq;
    using System.Linq.Expressions;
    using ChemCloud.Web.Models;
    using System.Web;    
    using System.Collections.Specialized;
    using System.Configuration;
    using ChemCloud;
    using ChemCloud.Web;
    using ChemCloud.Web.Framework;
    using ChemCloud.Web.Controllers;
    using ChemCloud.Core.Helper;
    using ChemCloud.IServices;
    using ChemCloud.Model;
    /// <summary>
    /// 消息控制器
    /// </summary>
    public class ChatMessageController : Controller
    {

        /// <summary>
        /// 
        /// </summary>
        public UserMemberInfo CurrentUser
        {
            get
            {
                long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("ChemCloud-User"), "Web");
                if (num == 0)
                {
                    return null;
                }
                return ServiceHelper.Create<IMemberService>().GetMember(num);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public IPaltManager CurrentManager
        {
            get
            {
                IPaltManager platformManager = null;
                long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("Himall-PlatformManager"), "Admin");
                if (num != 0)
                {
                    platformManager = ServiceHelper.Create<IManagerService>().GetPlatformManager(num);
                }
                return platformManager;
            }
        }


        /// <summary>
        /// 当前人员Cookie
        /// </summary>
        /// <returns></returns>
        public bool WriteCurrentUserCookieToRequest()
        {

            if (this.CurrentUser != null)
            {
                HttpCookie ChatMessage_SendUserID = new HttpCookie("ChatMessage_SendUserID");
                ChatMessage_SendUserID.Value = this.CurrentUser.Id.ToString();
                ChatMessage_SendUserID.Expires = DateTime.Now.AddYears(1);

                Response.Cookies.Add(ChatMessage_SendUserID);

                HttpCookie ChatMessage_SendUserName = new HttpCookie("ChatMessage_SendUserName");
                ChatMessage_SendUserName.Value = this.CurrentUser.UserName.ToString();
                ChatMessage_SendUserName.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(ChatMessage_SendUserName);

                return true;
                    
            }

            return false;
        }


        public bool WriteCurrentManagerCookieToRequest()
        {

            if (this.CurrentManager != null)
            {
                HttpCookie ChatMessage_SendUserID = new HttpCookie("ChatMessage_SendUserID");
                ChatMessage_SendUserID.Value = this.CurrentManager.Id.ToString();
                ChatMessage_SendUserID.Expires = DateTime.Now.AddYears(1);

                Response.Cookies.Add(ChatMessage_SendUserID);

                HttpCookie ChatMessage_SendUserName = new HttpCookie("ChatMessage_SendUserName");
                ChatMessage_SendUserName.Value = this.CurrentManager.UserName.ToString();
                ChatMessage_SendUserName.Expires = DateTime.Now.AddYears(1);



                Response.Cookies.Add(ChatMessage_SendUserName);

                return true;

            }

            return false;
        }


        [HttpGet()]
        /// <summary>
        /// 主界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {


            if (this.CurrentManager == null && this.CurrentUser==null)
            {
               Response.Redirect("/Login");

                return null;
                //return RedirectToRoute(new { controller = "Login", action = "Index" });//可跳出本controller
                //return this.RedirectToAction("Index", "Login");
                 
            }
             
            if (!this.WriteCurrentUserCookieToRequest())
            {
                this.WriteCurrentManagerCookieToRequest();
            }


            int i = 0;

            return this.View("~/Views/Chat/MessageHallPage.cshtml");
        }

      
 

      
        /// <summary>
        /// 最近的聊天记录
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="receiver">接收才</param>
        /// <param name="max">最大返回数</param>
        /// <returns></returns>
        /// string sender,string receiver,int max
        [HttpGet()]
        public ActionResult GetRecentMessageRecordPage(string sendUserID, string reviceUserID, int maxLimit)
        {

  
            var data = Data.ChatMessage.GetMessageRecords(sendUserID, reviceUserID, maxLimit);


            foreach (var item in data)
            {
                if (item.SendUserID.ToString() == sendUserID)
                {
                    item.SendPrimary = 1;
                }
            }
             

            return this.PartialView("~/Views/Chat/RecentMessageRecordPage.cshtml", data);
          
        }
        /// <summary>
        /// 1:近三个月，2：近六个月，3：一年内,4:三年内
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime GetHistroyMessageRecordBeginTime(int type)
        {

            DateTime beginTime = DateTime.Now;

            ///这里只有这个地方用，所有没有定义
            if (type == 1)
            {
                beginTime = DateTime.Now.AddMonths(-3);
            }

            if (type == 2)
            {
                beginTime = DateTime.Now.AddMonths(-6);
            }

            if (type == 3)
            {
                beginTime = DateTime.Now.AddYears(-1);
            }

            if (type == 4)
            {
                beginTime = DateTime.Now.AddYears(-3);
            }


            return beginTime;
        }

        /// <summary>
        /// 历史的聊天记录
        /// </summary>
        /// <param name="sendUserID"></param>
        /// <param name="reviceUserID"></param>
        /// <param name="type">1:近三个月，2：近六个月，3：一年内,4:三年内</param>
        /// <returns></returns>
        public ActionResult GetHistroyMessageRecordPage(string sendUserID, string reviceUserID, int type)
        {

            DateTime beginTime = ChatMessageController.GetHistroyMessageRecordBeginTime(type);

            var data = Data.ChatMessage.GetMessageRecords(sendUserID, reviceUserID, beginTime, DateTime.Now.AddHours(2));

            ///来自 发送者的
            foreach (var item in data)
            {
                if (item.SendUserID.ToString() == sendUserID)
                {
                    item.SendPrimary = 1;
                }
            }

            return this.PartialView("~/Views/Chat/HistroyMessageRecordPage.cshtml", data);
 
        }

        
        /// <summary>
        /// 保存消息到数据库
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveMessageRecord(
           Models.MessageViewModel item )
        {

            item.SendTime = DateTime.Now;
            Data.ChatMessage.SaveMessageRecord(item);

            return Json("11");
        }
    }
}



