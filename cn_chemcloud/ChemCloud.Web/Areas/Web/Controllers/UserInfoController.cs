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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using ChemCloud.Web.Areas.SellerAdmin.Models;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class UserInfoController : BaseMemberController
    {
        public UserInfoController()
        {
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldpassword, string password)
        {
            if (string.IsNullOrWhiteSpace(oldpassword) || string.IsNullOrWhiteSpace(password))
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "密码不能为空！"
                };
                return Json(result);
            }
            UserMemberInfo currentUser = base.CurrentUser;
            if (SecureHelper.MD5(string.Concat(SecureHelper.MD5(oldpassword), currentUser.PasswordSalt)) != currentUser.Password)
            {
                Result result1 = new Result()
                {
                    success = false,
                    msg = "旧密码错误"
                };
                return Json(result1);
            }
            ServiceHelper.Create<IMemberService>().ChangePassWord(currentUser.Id, password);
            Result result2 = new Result()
            {
                success = true,
                msg = "修改成功"
            };
            return Json(result2);
        }

        [HttpPost]
        public ActionResult CheckCode(string pluginId, string code, string destination)
        {
            string str = CacheKeyCollection.MemberPluginCheck(base.CurrentUser.UserName, pluginId);
            object obj = Cache.Get(str);
            UserMemberInfo currentUser = base.CurrentUser;
            if (obj == null || !(obj.ToString() == code))
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "验证码不正确或者已经超时"
                };
                return Json(result);
            }
            Cache.Remove(CacheKeyCollection.MemberPluginCheck(base.CurrentUser.UserName, pluginId));
            string str1 = string.Concat("Rebind", currentUser.Id);
            DateTime now = DateTime.Now;
            Cache.Insert(str1, "step2", now.AddMinutes(30));
            return Json(new { success = true, msg = "验证正确", key = currentUser.Id });
        }

        public JsonResult CheckOldPassWord(string password)
        {
            UserMemberInfo currentUser = base.CurrentUser;
            string str = SecureHelper.MD5(string.Concat(SecureHelper.MD5(password), currentUser.PasswordSalt));
            if (currentUser.Password == str)
            {
                return Json(new Result()
                {
                    success = true
                });
            }
            return Json(new Result()
            {
                success = false
            });
        }

        [HttpPost]
        public JsonResult GetCurrentUserInfo()
        {
            UserMemberInfo currentUser = base.CurrentUser;
            return Json(new { success = true, name = (string.IsNullOrWhiteSpace(currentUser.Nick) ? currentUser.UserName : currentUser.Nick) });
        }

        public ActionResult Index()
        {
            MemberDetail MemberDetail = ServiceHelper.Create<IMemberDetailService>().GetHimall_MemberDetail(base.CurrentUser.Id, false);
            if (MemberDetail == null)
            {
                MemberDetail = new MemberDetail();
                MemberDetail.CityRegionId = 0;
                MemberDetail.CompanyAddress = "";
                MemberDetail.CompanyName = "";
                MemberDetail.CompanySign = "";
                MemberDetail.CompanyProveFile = "";
                MemberDetail.MemberId = base.CurrentUser.Id;
                MemberDetail.CompanyIntroduction = "";
                MemberDetail.ZipCode = "";
                MemberDetail.Email = base.CurrentUser.Email;
                MemberDetail.MemberName = base.CurrentUser.UserName;
            }

            //图片上传路径修改 2016年4月25日
            ////公司/个人标志（图片）
            //string empty = string.Empty;
            //for (int i = 1; i < 4; i++)
            //{
            //    if (System.IO.File.Exists(Server.MapPath(string.Concat(MemberDetail.CompanySign, string.Format("{0}.png", i)))))
            //    {
            //        empty = string.Concat(empty, MemberDetail.CompanySign, string.Format("{0}.png", i), ",");
            //    }
            //}
            ////公司/个人证明性文件（图片）
            //string str = string.Empty;
            //for (int j = 1; j < 4; j++)
            //{
            //    if (System.IO.File.Exists(Server.MapPath(string.Concat(MemberDetail.CompanyProveFile, string.Format("{0}.png", j)))))
            //    {
            //        str = string.Concat(str, MemberDetail.CompanyProveFile, string.Format("{0}.png", j), ",");
            //    }
            //}
            ViewBag.CompanyRegionIds = ServiceHelper.Create<IRegionService>().GetRegionIdPath(MemberDetail.CityRegionId);
            string refuseReason = "";
            if (MemberDetail.Stage == ShopInfo.ShopAuditStatus.Refuse)
            {
                refuseReason = MemberDetail.RefuseReason;
            }
            ViewBag.RefuseReason = refuseReason;
            return View(MemberDetail);

        }

        public ActionResult ReBind(string pluginId)
        {
            Plugin<IMessagePlugin> plugin = PluginsManagement.GetPlugin<IMessagePlugin>(pluginId);
            ViewBag.ShortName = plugin.Biz.ShortName;
            ViewBag.id = pluginId;
            ViewBag.ContactInfo = ServiceHelper.Create<IMessageService>().GetDestination(base.CurrentUser.Id, pluginId, MemberContactsInfo.UserTypes.General);
            return View();
        }

        public ActionResult ReBindStep2(string pluginId, string key)
        {
            if (Cache.Get(string.Concat("Rebind", key)) as string != "step2")
            {
                RedirectToAction("ReBind", new { pluginId = pluginId });
            }
            Plugin<IMessagePlugin> plugin = PluginsManagement.GetPlugin<IMessagePlugin>(pluginId);
            ViewBag.ShortName = plugin.Biz.ShortName;
            ViewBag.id = pluginId;
            ViewBag.ContactInfo = ServiceHelper.Create<IMessageService>().GetDestination(base.CurrentUser.Id, pluginId, MemberContactsInfo.UserTypes.General);
            return View();
        }

        public ActionResult ReBindStep3(string name)
        {
            ViewBag.ShortName = name;
            return View();
        }

        [HttpPost]
        public ActionResult SendCode(string pluginId, string destination)
        {
            if (Cache.Get(CacheKeyCollection.MemberPluginReBindTime(base.CurrentUser.UserName, pluginId)) != null)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "120秒内只允许请求一次，请稍后重试!"
                };
                return Json(result);
            }
            int num = (new Random()).Next(10000, 99999);
            DateTime dateTime = DateTime.Now.AddMinutes(15);
            Cache.Insert(CacheKeyCollection.MemberPluginCheck(base.CurrentUser.UserName, pluginId), num, dateTime);
            MessageUserInfo messageUserInfo = new MessageUserInfo()
            {
                UserName = base.CurrentUser.UserName,
                SiteName = base.CurrentSiteSetting.SiteName,
                CheckCode = num.ToString()
            };
            ServiceHelper.Create<IMessageService>().SendMessageCode(destination, pluginId, messageUserInfo);
            string str = CacheKeyCollection.MemberPluginReBindTime(base.CurrentUser.UserName, pluginId);
            DateTime now = DateTime.Now;
            Cache.Insert(str, "0", now.AddSeconds(110));
            Result result1 = new Result()
            {
                success = true,
                msg = "发送成功"
            };
            return Json(result1);
        }

        [HttpPost]
        public ActionResult SendCodeStep2(string pluginId, string destination)
        {
            if (Cache.Get(CacheKeyCollection.MemberPluginReBindStepTime(base.CurrentUser.UserName, pluginId)) != null)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "120秒内只允许请求一次，请稍后重试!"
                };
                return Json(result);
            }
            int num = (new Random()).Next(10000, 99999);
            DateTime dateTime = DateTime.Now.AddMinutes(15);
            Cache.Insert(CacheKeyCollection.MemberPluginCheck(base.CurrentUser.UserName, pluginId), num, dateTime);
            MessageUserInfo messageUserInfo = new MessageUserInfo()
            {
                UserName = base.CurrentUser.UserName,
                SiteName = base.CurrentSiteSetting.SiteName,
                CheckCode = num.ToString()
            };
            ServiceHelper.Create<IMessageService>().SendMessageCode(destination, pluginId, messageUserInfo);
            string str = CacheKeyCollection.MemberPluginReBindStepTime(base.CurrentUser.UserName, pluginId);
            DateTime now = DateTime.Now;
            Cache.Insert(str, "0", now.AddSeconds(110));
            Result result1 = new Result()
            {
                success = true,
                msg = "发送成功"
            };
            return Json(result1);
        }

        public JsonResult UpdateUserInfo(UserMemberInfo model)
        {
            model.Id = base.CurrentUser.Id;
            ServiceHelper.Create<IMemberService>().UpdateMember(model);
            Result result = new Result()
            {
                success = true,
                msg = "修改成功"
            };
            return Json(result);
        }

        /// <summary>
        /// 编辑买家的资质信息
        /// </summary>
        /// <param name="_MemberDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditMemberDetail(MemberDetailAddQuery DetailJson)
        {
            MemberDetail _MemberDetail = new MemberDetail();
            _MemberDetail.MemberId = base.CurrentUser.Id;
            _MemberDetail.CompanyName = DetailJson.CompanyName;
            _MemberDetail.CityRegionId = DetailJson.CityRegionId;
            _MemberDetail.CompanyAddress = DetailJson.CompanyAddress;
            _MemberDetail.CompanySign = DetailJson.CompanySign;
            _MemberDetail.CompanyProveFile = DetailJson.CompanyProveFile;
            _MemberDetail.CompanyIntroduction = DetailJson.CompanyIntroduction;
            _MemberDetail.ZipCode = DetailJson.ZipCode;
            ServiceHelper.Create<IMemberDetailService>().UpdateHimall_MemberDetail(_MemberDetail);
            return Json(new { success = true });
        }
    }
}