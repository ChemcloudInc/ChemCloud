using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins;
using ChemCloud.Core.Plugins.OAuth;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Web;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class LoginController : BaseController
    {
        private const int TIMES_WITHOUT_CHECKCODE = 3;

        public LoginController()
        {
        }

        [HttpPost]
        public JsonResult BindUser(string username, string password, string oauthId, string openId, string unionid = null, string unionopenid = null)
        {
            
            IMemberService memberService = ServiceHelper.Create<IMemberService>();
            UserMemberInfo userMemberInfo = memberService.Login(username, password);
            if (userMemberInfo == null)
            {
                throw new HimallException("用户名和密码不匹配");
            }
            memberService.BindMember(userMemberInfo.Id, oauthId, openId, null, unionid, unionopenid);
            string str = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");
            WebHelper.SetCookie("ChemCloud-User", str);
            return Json(new { success = true });
        }

        public ActionResult BindUser(string oauthId, string openId, string name, string unionid = null, string unionopenid = null)
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.OauthId = oauthId;
            ViewBag.NickName = name;
            ViewBag.OpenId = openId;
            base.ViewBag.unionid = (unionid == null ? string.Empty : unionid);
            base.ViewBag.unionopenid = (unionopenid == null ? string.Empty : unionopenid);
            IOAuthPlugin biz = PluginsManagement.GetPlugin<IOAuthPlugin>(oauthId).Biz;
            ViewBag.ServiceProvider = biz.ShortName;
            return View();
        }

        private void CheckCheckCode(string username, string checkCode)
        {
            if (GetErrorTimes(username) >= 3)
            {
                if (string.IsNullOrWhiteSpace(checkCode))
                {
                    throw new LoginException("30分钟内登录错误3次以上需要提供验证码", LoginException.ErrorTypes.CheckCodeError);
                }
                if (base.Session["checkCode"]!=null&&(base.Session["checkCode"] as string).ToLower() != checkCode.ToLower())
                {
                    throw new LoginException("验证码错误", LoginException.ErrorTypes.CheckCodeError);
                }
                base.Session["checkCode"] = Guid.NewGuid().ToString();
            }
        }

        [HttpPost]
        public JsonResult CheckCode(string checkCode)
        {
            JsonResult jsonResult;
            try
            {
                string item = base.Session["checkCode"] as string;
                bool lower = item.ToLower() == checkCode.ToLower();
                jsonResult = Json(new { success = lower });
            }
            catch (HimallException himallException1)
            {
                HimallException himallException = himallException1;
                jsonResult = Json(new { success = false, msg = himallException.Message });
            }
            catch (Exception exception)
            {
                Log.Error("检验验证码时发生异常", exception);
                jsonResult = Json(new { success = false, msg = "未知错误" });
            }
            return jsonResult;
        }

        private void CheckInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new LoginException("请填写用户名", LoginException.ErrorTypes.UsernameError);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new LoginException("请填写密码", LoginException.ErrorTypes.PasswordError);
            }
        }

        private void ClearErrorTimes(string username)
        {
            Cache.Remove(CacheKeyCollection.MemberLoginError(username));
        }

        public ActionResult GetCheckCode()
        {
            string str;
            MemoryStream memoryStream = ImageHelper.GenerateCheckCode(out str);
            base.Session["checkCode"] = str;
            return base.File(memoryStream.ToArray(), "image/png");
        }

        [HttpPost]
        public JsonResult GetErrorLoginTimes(string username)
        {
            return Json(new { errorTimes = GetErrorTimes(username) });
        }

        private int GetErrorTimes(string username)
        {
            object obj = Cache.Get(CacheKeyCollection.MemberLoginError(username));
            return (obj == null ? 0 : (int)obj);
        }

        public ActionResult Index()
        {
            ViewBag.SiteName = base.CurrentSiteSetting.SiteName;
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            IEnumerable<Plugin<IOAuthPlugin>> plugins = PluginsManagement.GetPlugins<IOAuthPlugin>(true);
            string str = string.Concat(base.Request.Url.Scheme, "://", base.Request.Url.Host, (base.Request.Url.Port == 80 ? "" : string.Concat(":", base.Request.Url.Port)));
            string mapPath = IOHelper.GetMapPath("/");
            IEnumerable<OAuthInfo> oAuthInfo =
                from item in plugins
                select new OAuthInfo()
                {
                    Name = item.Biz.ShortName,
                    Url = item.Biz.GetOpenLoginUrl(string.Concat(str, "/Login/OauthCallBack?oauthId=", item.PluginInfo.PluginId)),
                    LogoDefault = item.Biz.Icon_Default.Replace(mapPath, "/"),
                    LogoHover = item.Biz.Icon_Hover.Replace(mapPath, "/")
                };
            return View(oAuthInfo);
        }

        /// <summary>
        /// 验证账户后，自动登录
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionResult FromEmailVerification(string username, string password)
        {

            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(username);
            if (userMemberInfo.Disabled)
            {
                return View(new UserMemberInfo());
            }
            if (userMemberInfo != null && !string.IsNullOrEmpty(username) && userMemberInfo.Password == password && userMemberInfo.UserName == username)
            {
                userMemberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(username);
                if (userMemberInfo != null)
                {
                    userMemberInfo.Disabled = true;
                    ServiceHelper.Create<IMemberService>().UpdateMember(userMemberInfo);
                }
                if (userMemberInfo.UserType == 2)
                {
                    BizAfterLogin.Run(userMemberInfo.Id);
                    string str = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "SellerAdmin");
                    string str1 = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");

                    WebHelper.SetCookie("ChemCloud-SellerManager", str);
                    WebHelper.SetCookie("ChemCloud-User", str1);

                }
                else if (userMemberInfo.UserType == 3)
                {
                    BizAfterLogin.Run(userMemberInfo.Id);
                    string str = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");
                    WebHelper.SetCookie("ChemCloud-User", str);
                }
            }
            return View(userMemberInfo);
        }


        [HttpPost]
        public JsonResult Login(string username, string password, string checkCode, bool keep )
        {
            JsonResult jsonResult = null;
            try
            {
                CheckInput(username, password);
                CheckCheckCode(username, checkCode);
                int result = SellerLoginIn(username, password, keep);
                if (result == 2)
                {
                    ManagerInfo managerInfo = ServiceHelper.Create<IManagerService>().Login(username, password, false);
                    ClearErrorTimes(username);
                    jsonResult = Json(new { success = true, IsChildSeller = true, userId = UserCookieEncryptHelper.Encrypt(managerInfo.Id, "SellerAdmin") });
                }
                else if (result == 3)
                {
                    ClearErrorTimes(username);
                    jsonResult = Json(new { success = true, IsChildSeller = false });
                }

            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                int num2 = SetErrorTimes(username);
                Log.Error(string.Concat("用户", username, "登录时发生异常"), exception);
                jsonResult = Json(new { success = false, msg = "用户名或者密码错误！", errorTimes = num2, minTimesWithoutCheckCode = 3 });
            }
            return jsonResult;
        }

        public ActionResult OauthCallBack(string oauthId)
        {
            Plugin<IOAuthPlugin> plugin = PluginsManagement.GetPlugin<IOAuthPlugin>(oauthId);
            OAuthUserInfo userInfo = plugin.Biz.GetUserInfo(base.Request.QueryString);
            UserMemberInfo memberByUnionId = null;
            if (!string.IsNullOrWhiteSpace(userInfo.UnionId))
            {
                memberByUnionId = ServiceHelper.Create<IMemberService>().GetMemberByUnionId(oauthId, userInfo.UnionId);
            }
            if (memberByUnionId == null && !string.IsNullOrWhiteSpace(userInfo.OpenId))
            {
                memberByUnionId = ServiceHelper.Create<IMemberService>().GetMemberByOpenId(oauthId, userInfo.OpenId);
            }
            if (memberByUnionId != null)
            {
                SellerLoginIn(memberByUnionId.UserName, memberByUnionId.Password, false);
                string str = UserCookieEncryptHelper.Encrypt(memberByUnionId.Id, "Web");
                WebHelper.SetCookie("ChemCloud-User", str.ToString());
                BizAfterLogin.Run(memberByUnionId.Id);
                return Redirect("/");
            }
            if (string.IsNullOrWhiteSpace(userInfo.OpenId))
            {
                return View();
            }
            object[] objArray = new object[] { oauthId, userInfo.OpenId, userInfo.NickName, userInfo.UnionId, userInfo.OpenId };
            return Redirect(string.Format("/Login/BindUser?oauthId={0}&openId={1}&name={2}&unionid={3}&unionopenid={4}", objArray));
        }

        //private ManagerInfo SellerLoginIn(string username, string password, bool keep = false)
        //{
        //    ManagerInfo managerInfo = ServiceHelper.Create<IManagerService>().Login(username, password, false);
        //    if (managerInfo == null)
        //    {
        //        return null;
        //    }
        //    string str = UserCookieEncryptHelper.Encrypt(managerInfo.Id, "SellerAdmin");
        //    if (!keep)
        //    {
        //        WebHelper.SetCookie("ChemCloud-SellerManager", str);
        //    }
        //    else
        //    {
        //        DateTime now = DateTime.Now;
        //        WebHelper.SetCookie("ChemCloud-SellerManager", str, now.AddDays(7));
        //    }
        //    return managerInfo;
        //}

        /// <summary>
        /// 卖家和平台管理元登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="keep"></param>
        /// <returns></returns>
        private int SellerLoginIn(string username, string password, bool keep )
        {
            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().Login(username, password);
            if (userMemberInfo == null)
            {
                throw new LoginException("用户名和密码不匹配", LoginException.ErrorTypes.PasswordError);
            }

            if (userMemberInfo.UserType == 2)
            {
                if (userMemberInfo.Disabled == false)
                {
                    throw new LoginException("请到邮箱验证用户", LoginException.ErrorTypes.UsernameError);
                }
                BizAfterLogin.Run(userMemberInfo.Id);
                string str = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "SellerAdmin");
                string str1 = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");
                if (!keep)
                {
                    WebHelper.SetCookie("ChemCloud-SellerManager", str);
                    WebHelper.SetCookie("ChemCloud-User", str1);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    WebHelper.SetCookie("ChemCloud-SellerManager", str, now.AddDays(7));
                    WebHelper.SetCookie("ChemCloud-User", str1, now.AddDays(7));
                }


                return 2;
            }
            else if (userMemberInfo.UserType == 3)
            {
                if (userMemberInfo.Disabled == false)
                {
                    throw new LoginException("用户未激活", LoginException.ErrorTypes.UsernameError);
                }
                BizAfterLogin.Run(userMemberInfo.Id);
                string str = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");
                if (!keep)
                {
                    WebHelper.SetCookie("ChemCloud-User", str);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    WebHelper.SetCookie("ChemCloud-User", str, now.AddDays(7));
                }
                if (userMemberInfo.ParentSellerId == 0)
                {
                    string name = "";
                    PurchaseRolesInfo roleInfo = ServiceHelper.Create<IPermissionGroupService>().GetPurchaseRoleByUserId(userMemberInfo.Id);
                    if (roleInfo == null)
                    {
                        long Languagetype = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
                        if (Languagetype == 1)
                            name = "管理员";
                        if (Languagetype == 2)
                            name = "Admin";
                        PurchaseRolesInfo roleInfos = ServiceHelper.Create<IPermissionGroupService>().AddPermissionGroup(userMemberInfo.Id, name);
                        if (roleInfos.Id != 0)
                        {//用户添加成功 添加组织结构
                            Organization oinfo = new Organization()
                            {
                                UserId = userMemberInfo.Id,
                                RoleId = roleInfos.Id,
                                RoleName = roleInfos.RoleName,
                                ParentRoleId = 0,
                                ParentId = 0
                            };
                            ServiceHelper.Create<IOrganizationService>().AddOrganization(oinfo);
                        }
                    }
                }
                return 3;
            }
            else
            {
                return -1;
            }

        }



        private int SetErrorTimes(string username)
        {
            int errorTimes = GetErrorTimes(username) + 1;
            string str = CacheKeyCollection.MemberLoginError(username);
            object obj = errorTimes;
            DateTime now = DateTime.Now;
            Cache.Insert(str, obj, now.AddMinutes(30));
            return errorTimes;
        }

        [HttpPost]
        public JsonResult Skip(string oauthId, string openId, string nickName, string unionid = null, string unionopenid = null)
        {
            string str = DateTime.Now.ToString("yyMMddHHmmssffffff");
            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().QuickRegister(str, string.Empty, nickName, oauthId, openId, null, MemberOpenIdInfo.AppIdTypeEnum.Normal, unionid, unionopenid);
            string str1 = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");
            WebHelper.SetCookie("ChemCloud-User", str1);
            return Json(new { success = true });
        }

        private UserMemberInfo UserLoginIn(string username, string password, bool keep = false)
        {
            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().Login(username, password);
            if (userMemberInfo == null)
            {
                throw new LoginException("用户名和密码不匹配", LoginException.ErrorTypes.PasswordError);
            }
            BizAfterLogin.Run(userMemberInfo.Id);
            string str = UserCookieEncryptHelper.Encrypt(userMemberInfo.Id, "Web");
            if (!keep)
            {
                WebHelper.SetCookie("ChemCloud-User", str);
            }
            else
            {
                DateTime now = DateTime.Now;
                WebHelper.SetCookie("ChemCloud-User", str, now.AddDays(7));
            }

            return userMemberInfo;
        }
    }
}