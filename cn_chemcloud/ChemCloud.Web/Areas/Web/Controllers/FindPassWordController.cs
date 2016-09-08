using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class FindPassWordController : BaseWebController
    {
        public FindPassWordController()
        {
        }

        public ActionResult ChangePassWord(string passWord, string key)
        {
            UserMemberInfo userMemberInfo = Cache.Get(key) as UserMemberInfo;
            if (userMemberInfo == null)
            {
                return Json(new { success = false, flag = -1, msg = "验证超时" });
            }
            long id = userMemberInfo.Id;
            ServiceHelper.Create<IMemberService>().ChangePassWord(id, passWord);
            MessageUserInfo messageUserInfo = new MessageUserInfo()
            {
                SiteName = base.CurrentSiteSetting.SiteName,
                UserName = userMemberInfo.UserName
            };
            return Json(new { success = true, flag = 1, msg = "成功找回密码" });
        }

        [HttpPost]
        public ActionResult CheckPluginCode(string pluginId, string code, string key)
        {
            UserMemberInfo userMemberInfo = Cache.Get(key) as UserMemberInfo;
            string str = CacheKeyCollection.MemberFindPassWordCheck(userMemberInfo.UserName, pluginId);
            object obj = Cache.Get(str);
            if (obj == null || !(obj.ToString() == code))
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "验证码不正确或者已经超时"
                };
                return Json(result);
            }
            Cache.Remove(CacheKeyCollection.MemberFindPassWordCheck(userMemberInfo.UserName, pluginId));
            string str1 = string.Concat(key, "3");
            DateTime now = DateTime.Now;
            Cache.Insert(str1, userMemberInfo, now.AddMinutes(15));
            return Json(new { success = true, msg = "验证正确", key = key });
        }

        [HttpPost]
        public ActionResult CheckUser(string userName, string checkCode)
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString().Replace("-", "");
            VaildCode(checkCode);
            UserMemberInfo memberByContactInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(userName);
            if (memberByContactInfo == null)
            {
                return Json(new { success = false, tag = "username", msg = "您输入的账户名不存在，请核对后重新输入" });
            }
            DateTime now = DateTime.Now;
            Cache.Insert(str, memberByContactInfo, now.AddMinutes(15));
            return Json(new { success = true, key = str });
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult GetCheckCode()
        {
            string str;
            MemoryStream memoryStream = ImageHelper.GenerateCheckCode(out str);
            base.Session["FindPassWordcheckCode"] = str;
            return base.File(memoryStream.ToArray(), "image/png");
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SendMail(string username)
        {
            UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(username);
            string validCode = StringHelper.GenerateCheckCode();
            SaveCookie("validCode", validCode, 0);
   
            bool falg = ChemCloud.Service.SendMail.SendEmail(userInfo.Email, "Please validate Your Email", "hello,<span style=\"color:green;\">" + username + "</span><br/>Welcome to ChemCloud!<br/> Your ChemCloud ValidCode is :<span style=\"color:red;\">" + validCode + "</span>");

            Result result = new Result();
            result.msg = falg.ToString();
            result.success = falg;
            return Json(result);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="cookieValue">Cookie值</param>
        /// <param name="cookieTime">Cookie过期时间(分钟),0为关闭页面失效</param>
        /// <param name="isencryption">Cookie是否加密</param>
        /// <param name="cookiepath">Cookie路径</param>
        public static void SaveCookie(string cookieName, string cookieValue, double cookieTime, bool isencryption = false, string cookiepath = "/")
        {

            var myCookie = new HttpCookie(cookieName);
            DateTime now = DateTime.Now;
            myCookie.Value = cookieValue;
            myCookie.Path = cookiepath;
            if (Math.Abs(cookieTime) > 0)
            {
                myCookie.Expires = now.AddHours(cookieTime);
                if (System.Web.HttpContext.Current.Response.Cookies[cookieName] != null)
                {
                    System.Web.HttpContext.Current.Response.Cookies.Remove(cookieName);
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                if (System.Web.HttpContext.Current.Response.Cookies[cookieName] != null)
                {
                    System.Web.HttpContext.Current.Response.Cookies.Remove(cookieName);
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        [HttpPost]
        public ActionResult SendCode(string pluginId, string key)
        {
            UserMemberInfo userMemberInfo = Cache.Get(key) as UserMemberInfo;
            if (userMemberInfo == null)
            {
                return Json(new { success = false, flag = -1, msg = "验证已超时！" });
            }
            string destination = ServiceHelper.Create<IMessageService>().GetDestination(userMemberInfo.Id, pluginId, MemberContactsInfo.UserTypes.General);
            if (Cache.Get(CacheKeyCollection.MemberPluginFindPassWordTime(userMemberInfo.UserName, pluginId)) != null)
            {
                return Json(new { success = false, flag = 0, msg = "120秒内只允许请求一次，请稍后重试!" });
            }
            int num = (new Random()).Next(10000, 99999);
            DateTime dateTime = DateTime.Now.AddMinutes(15);
            Cache.Insert(CacheKeyCollection.MemberFindPassWordCheck(userMemberInfo.UserName, pluginId), num, dateTime);
            MessageUserInfo messageUserInfo = new MessageUserInfo()
            {
                UserName = userMemberInfo.UserName,
                SiteName = base.CurrentSiteSetting.SiteName,
                CheckCode = num.ToString()
            };
            ServiceHelper.Create<IMessageService>().SendMessageCode(destination, pluginId, messageUserInfo);
            string str = CacheKeyCollection.MemberPluginFindPassWordTime(userMemberInfo.UserName, pluginId);
            DateTime now = DateTime.Now;
            Cache.Insert(str, "0", now.AddSeconds(110));
            return Json(new { success = true, flag = 1, msg = "发送成功" });
        }

        public ActionResult Step2(string key)
        {
            UserMemberInfo userMemberInfo = Cache.Get(key) as UserMemberInfo;
            if (userMemberInfo == null)
            {
                return RedirectToAction("Error", "FindPassWord");
            }
            ViewBag.UserName = userMemberInfo.UserName;
            ViewBag.Key = key;
            return View(userMemberInfo);
        }

        public ActionResult Step3(string key)
        {
            UserMemberInfo userMemberInfo = Cache.Get(key) as UserMemberInfo;
            if (userMemberInfo == null)
            {
                return RedirectToAction("Error", "FindPassWord");
            }
            ViewBag.Key = key;
            return View();
        }

        public ActionResult Step4()
        {
            return View();
        }

        private void VaildCode(string checkCode)
        {
            if (string.IsNullOrWhiteSpace(checkCode))
            {
                throw new HimallException("验证码不能为空");
            }
            else
            {
                string item = base.Session["FindPassWordcheckCode"] as string;
                if (string.IsNullOrEmpty(item))
                {
                    throw new HimallException("验证码超时，请刷新");
                }
                else
                {
                    if (checkCode == "9999")
                    {
                        //万能验证码
                    }
                    else
                    {
                        if (item.ToLower() != checkCode.ToLower())
                        {
                            throw new HimallException("验证码不正确");
                        }
                        else
                        {
                            base.Session["FindPassWordcheckCode"] = Guid.NewGuid().ToString();
                        }
                    }
                }
            }
        }
    }
}