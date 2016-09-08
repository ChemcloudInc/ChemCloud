using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Core.Plugins.Message;
using ChemCloud.IServices;
using ChemCloud.Model;
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
    public class RegisterController : BaseController
    {
        private const string CHECK_CODE_KEY = "regist_CheckCode";

        public RegisterController()
        {
        }
        public UserMemberInfo GetMember()
        {
            long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("ChemCloud-User"), "Web");
            if (num == 0)
            {
                return null;
            }
            return ServiceHelper.Create<IMemberService>().GetMember(num);
        }
        [HttpPost]
        public JsonResult CheckCheckCode(string checkCode)
        {
            return Json(new { success = true, result = (base.Session["regist_CheckCode"] == null ? false : checkCode.ToLower() == base.Session["regist_CheckCode"].ToString().ToLower()) });
        }

        [HttpPost]
        public JsonResult CheckCode(string pluginId, string code, string destination)
        {
            object obj = Cache.Get(CacheKeyCollection.MemberPluginCheck(destination, pluginId));
            if (obj != null && obj.ToString() == code)
            {
                Result result = new Result()
                {
                    success = true,
                    msg = "验证正确"
                };
                return Json(result);
            }
            Result result1 = new Result()
            {
                success = false,
                msg = "验证码不正确或者已经超时"
            };
            return Json(result1);
        }

        [HttpPost]
        public JsonResult CheckManagerUser(string username)
        {
            bool flag = ServiceHelper.Create<IManagerService>().CheckUserNameExist(username, false);
            return Json(new { success = true, result = flag });
        }

        [HttpPost]
        public JsonResult CheckMobile(string mobile)
        {
            bool flag = ServiceHelper.Create<IMemberService>().CheckMobileExist(mobile);
            return Json(new { success = true, result = flag });
        }

        [HttpPost]
        public JsonResult CheckUserName(string username)
        {
            bool flag = ServiceHelper.Create<IMemberService>().CheckMemberExist(username);
            return Json(new { success = true, result = flag });
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool flag = ServiceHelper.Create<IMemberService>().CheckEmailExist(email);
            return Json(new { success = true, result = flag });
        }

        [ValidateInput(false)]
        public ActionResult GetCheckCode()
        {
            string str;
            MemoryStream memoryStream = ImageHelper.GenerateCheckCode(out str);
            base.Session["regist_CheckCode"] = str;
            return base.File(memoryStream.ToArray(), "image/png");
        }

        public ActionResult Index(long id = 0L)
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.MobileVerifOpen = base.CurrentSiteSetting.MobileVerifOpen;
            ViewBag.Introducer = id;
            ViewBag.CoinDics = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(1);
            return View();
        }

        public ActionResult Index_(long id = 0L)
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.MobileVerifOpen = base.CurrentSiteSetting.MobileVerifOpen;
            ViewBag.Introducer = id;

            return View();
        }

        /// <summary>
        /// 添加子账户
        /// </summary>
        /// <param name="currentuserid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RegisterChild(long currentuserid, long id = 0L)
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.MobileVerifOpen = base.CurrentSiteSetting.MobileVerifOpen;
            ViewBag.Introducer = currentuserid;
            return View();
        }

        [HttpGet]
        public ActionResult RegBusiness()
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            return View();
        }

        public ActionResult RegisterAgreement()
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;

            return View();
        }

        public JsonResult RegisterAgreememtInfo()
        {
            return Json(new
            {
                AgreementContent = ServiceHelper.Create<ISystemAgreementService>().GetAgreement(AgreementInfo.AgreementTypes.Buyers).AgreementContent,
                AgreementContent1 = GetSellerAgreement(),
                success = true
            });
        }

        public string GetSellerAgreement()
        {
            AgreementInfo agreement = ServiceHelper.Create<ISystemAgreementService>().GetAgreement(AgreementInfo.AgreementTypes.Seller);
            if (agreement == null)
            {
                return "";
            }
            return agreement.AgreementContent;
        }

        /// <summary>
        /// 采购商注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="registertype"></param>
        /// <param name="introducer"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegisterUser(string username, string password, string email, string registertype, long introducer = 0L)
        {
            bool result = false;
            string RoleName = "";
            long id = 0;
            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().Register(username, password, email, registertype, introducer);
            if (userMemberInfo != null)
            {
                base.Session.Remove("regist_CheckCode");
                result = true;
                id = userMemberInfo.Id;
                ServiceHelper.Create<ISiteMessagesService>().SendMemberRegisterMessage(userMemberInfo.Id);
            }
            if (id != 0)
            {
                if (int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()) == 1)
                    RoleName = "管理员";
                if (int.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString()) == 2)
                    RoleName = "Admin";
            }
            PurchaseRolesInfo rolesInfo = ServiceHelper.Create<IPermissionGroupService>().AddPermissionGroup(id, RoleName);
            if (rolesInfo.Id != 0)
            {//用户添加成功 添加组织结构
                Organization oinfo = new Organization()
                {
                    UserId = id,
                    RoleId = rolesInfo.Id,
                    RoleName = rolesInfo.RoleName,
                    ParentRoleId = 0,
                    ParentId = 0
                };
                ServiceHelper.Create<IOrganizationService>().AddOrganization(oinfo);
            }
            return Json(new { success = result, memberId = id });
        }

        /// <summary>
        /// 供应商注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="registertype"></param>
        /// <param name="introducer"></param>
        /// <returns></returns>
        public JsonResult RegisterUser1(string username, string password, string email, string registertype, long introducer = 0L)
        {
            UserMemberInfo userMemberInfo = ServiceHelper.Create<IMemberService>().Register(username, password, email, registertype, introducer);
            if (userMemberInfo != null)
            {
                base.Session.Remove("regist_CheckCode");
                //供应商注册后，添加ChemCloud_Managers表信息
                ManagerInfo managerInfo = ServiceHelper.Create<IManagerService>().AddSellerManager(userMemberInfo.UserName, userMemberInfo.Password, userMemberInfo.PasswordSalt);
                if (managerInfo != null)
                {
                    ServiceHelper.Create<ISiteMessagesService>().SendSupplierRegisterMessage(userMemberInfo.Id);
                }
            }
            return Json(new { success = true, memberId = userMemberInfo.Id });
        }

        [HttpPost]
        public JsonResult SendCode(string pluginId, string destination)
        {
            ServiceHelper.Create<IMemberService>().CheckContactInfoHasBeenUsed(pluginId, destination, MemberContactsInfo.UserTypes.General);
            if (Cache.Get(CacheKeyCollection.MemberPluginCheckTime(destination, pluginId)) != null)
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
            if (pluginId.ToLower().Contains("email"))
            {
                dateTime = DateTime.Now.AddHours(24);
            }
            Cache.Insert(CacheKeyCollection.MemberPluginCheck(destination, pluginId), num, dateTime);
            MessageUserInfo messageUserInfo = new MessageUserInfo()
            {
                UserName = "",
                SiteName = base.CurrentSiteSetting.SiteName,
                CheckCode = num.ToString()
            };
            ServiceHelper.Create<IMessageService>().SendMessageCode(destination, pluginId, messageUserInfo);
            string str = CacheKeyCollection.MemberPluginCheckTime(destination, pluginId);
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
        public JsonResult ActiveEmail(string username)
        {
            bool falg = ChemCloud.Service.SendMail.SendEmail(username, "Please Active Your Account", "hello,<span style=\"color:green;\">" + username + "</span><br/>Welcome to ChemCloud!<br/> Your ChemCloud account ActiveCode is:<span style=\"color:red;\">085200</span>");
            Result result = new Result();
            result.msg = falg.ToString();
            result.success = falg;
            return Json(result);
        }

        [HttpPost]
        public JsonResult SendMail(string username, string email)
        {
            bool falg = false;
            if (ServiceHelper.Create<IMemberService>().GetMemberByName(username) != null)
            {
                string password = ServiceHelper.Create<IMemberService>().GetMemberByName(username).Password;

                int usertype = ServiceHelper.Create<IMemberService>().GetMemberByName(username).UserType;
                string mailsubject = "ChemCloud,Welcome To Join ChemCloud";

                string mailcontent = "Welcome To ChemCloud!";
                MessageSetting model = ServiceHelper.Create<IMessageSettingService>().GetSettingByMessageNameId(ChemCloud.Model.MessageSetting.MessageModuleStatus.RegisterMailContent);
                if (usertype == 2)
                {
                    model = ServiceHelper.Create<IMessageSettingService>().GetSettingByMessageNameId(ChemCloud.Model.MessageSetting.MessageModuleStatus.RegisterMailContent_GYS);
                }
                if (model != null)
                {
                    mailcontent = model.MessageContent == null ? mailcontent : model.MessageContent;
                }
                string str = mailcontent.Replace("@account", username).Replace("@username", username).Replace("@password", password);

                string currentrooturl = ChemCloud.Core.Common.GetRootUrl("");

                str = str.Replace("@httpurl", currentrooturl);
                falg = ChemCloud.Service.SendMail.SendEmail(email, mailsubject, str);
            }
            Result result = new Result();
            result.msg = falg.ToString();
            result.success = falg;
            return Json(result);
        }

        //注册成功，跳转到提示页面
        public ActionResult RegisterSuccessTip()
        {
            return View();
        }
    }
}