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

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class OrganizationController : BaseWebController
    {
        
        public ActionResult Management()
        {
            Organization Org = ServiceHelper.Create<IOrganizationService>().GetOrganizationByUserId(base.CurrentUser.Id);
            ViewBag.UserId = Org.UserId;
            List<Organization> OrgModel = ServiceHelper.Create<IOrganizationService>().GetOrganizations(Org.Id);
            return View(OrgModel);
        }
        public ActionResult AddNewOrg()
        {
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.MobileVerifOpen = base.CurrentSiteSetting.MobileVerifOpen;
            ViewBag.userId = base.CurrentUser.Id;
            ViewBag.Name = base.CurrentUser.UserName;
            ViewBag.parentId = base.CurrentUser.Id;
            ViewBag.LanguageType = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            return View();
        }
        
        public ActionResult Edit(long Id)
        {
            Organization Org = ServiceHelper.Create<IOrganizationService>().GetOrganizationById(Id);
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.MobileVerifOpen = base.CurrentSiteSetting.MobileVerifOpen;
            ViewBag.ParentRoleName = Org.ParentRoleName;
            ViewBag.RoleName = Org.RoleName;
            ViewBag.roleId = Org.RoleId;
            ViewBag.ParentRoleId = Org.ParentRoleId;
            ViewBag.MasterId = base.CurrentUser.Id;
            ViewBag.userId = Org.UserId;
            ViewBag.Id = Id;
            ViewBag.Name = Org.UserName;
            ViewBag.parentId = Org.ParentId;
            ViewBag.CurrentParentId = Org.ParentId;
            ViewBag.ParentName = Org.ParentName;
            UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMember(Org.UserId);
            ViewBag.FirstName = (userInfo == null ? "" : userInfo.FirstName);
            ViewBag.SecondName = (userInfo == null ? "" : userInfo.SecondName);
            ViewBag.LanguageType = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            return View();
        }
        public ActionResult AddByParent(long Id)
        {
            Organization Org = ServiceHelper.Create<IOrganizationService>().GetOrganizationById(Id);
            ViewBag.RoleName = Org.RoleName;
            ViewBag.userId = Org.UserId;
            ViewBag.parentId = Org.UserId;
            ViewBag.roleId = Org.RoleId;
            ViewBag.ParentName = Org.UserName;
            ViewBag.AdminUserId = base.CurrentUser.Id;
            ViewBag.Logo = base.CurrentSiteSetting.Logo;
            ViewBag.MobileVerifOpen = base.CurrentSiteSetting.MobileVerifOpen;
            ViewBag.LanguageType = long.Parse(System.Configuration.ConfigurationManager.AppSettings["Language"].ToString());
            return View();
        }
        [HttpPost]
        public JsonResult GetOrganization(long Id)
        {
            List<Organization> OrgModel = ServiceHelper.Create<IOrganizationService>().ChildOrganization(Id);
            return Json(new { Successfly = true, Organization = OrgModel });
        }
        [HttpPost]
        public JsonResult IsAdmin(long Id)
        {
            bool flag = ServiceHelper.Create<IOrganizationService>().IsAdmin(Id);
            return Json(new { Successfly = flag });
        }
        [HttpPost]
        public JsonResult AddOrganization(string username,long roleId,long parentroleId,long parentId)
        {
            string str1 = string.Concat(base.CurrentUser.UserName, ":", username);
            UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(str1);
            Organization Org = new Organization();
            Org.UserId = userInfo.Id;
            Org.RoleId = roleId;
            Org.ParentRoleId = parentroleId;
            Org.ParentId = parentId;
            bool flag = ServiceHelper.Create<IOrganizationService>().AddOrganization(Org);
            if(flag)
                return Json(new { success = true}); 
            else
                return Json(new { success = false});
        }
        [ValidateInput(false)]
        public ActionResult GetCheckCode()
        {
            string str;
            MemoryStream memoryStream = ImageHelper.GenerateCheckCode(out str);
            base.Session["regist_CheckCode"] = str;
            return base.File(memoryStream.ToArray(), "image/png");
        }

        private string GetPasswrodWithTwiceEncode(string password, string salt)
        {
            string str = SecureHelper.MD5(password);
            return SecureHelper.MD5(string.Concat(str, salt));
        }

        [HttpPost]
        public JsonResult AddMemberInfo(string username, string password, long parentIds, string email, string firstName, string secondName)
        {
            string str1 = string.Concat(base.CurrentUser.UserName, ":", username);
            password = password.Trim();
            Guid guid = Guid.NewGuid();
            string str = guid.ToString("N").Substring(12);
            password = GetPasswrodWithTwiceEncode(password, str);
            UserMemberInfo userMemberInfo = new UserMemberInfo()
            {
                UserName = str1,
                PasswordSalt = str,
                Password = password,
                CreateDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Email = email,
                Nick = str1,
                RealName = str1,
                CellPhone = "",
                Disabled = true,
                UserType = 3,
                ParentSellerId = parentIds,
                FirstName = firstName,
                SecondName = secondName
            };
            bool flag = ServiceHelper.Create<IMemberService>().RegisterChild(userMemberInfo);
            if (flag)
                return Json(new { success = true});
            else
                return Json(new { success = false});
        }
        [HttpPost]
        public JsonResult CheckUserName(string username)
        {
            string str1 = string.Concat(base.CurrentUser.UserName, ":", username);
            bool flag = ServiceHelper.Create<IMemberService>().CheckMemberExist(str1);
            return Json(new { success = true, result = flag });
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool flag = ServiceHelper.Create<IMemberService>().CheckEmailExist(email);
            return Json(new { success = true, result = flag });
        }
        [HttpPost]
        public JsonResult UpdateOrganization(long Id,long roleId,long ParentRoleId,long ParentId)
        {
            Organization Org = ServiceHelper.Create<IOrganizationService>().GetOrganizationById(Id);
            if(Org != null)
            {
                Org.RoleId = roleId;
                Org.ParentRoleId = ParentRoleId;
                Org.ParentId = ParentId;
                ServiceHelper.Create<IOrganizationService>().UpdateOrganization(Org);
            }
            return Json(new { success = true });
        }
        
        [HttpPost]
        public JsonResult UpdateLimitedAmount(string json)
        {
            List<bool> flags = new List<bool>();
            List<LimitedAmount> Limiteds = new List<LimitedAmount>();
            UserMemberInfo model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserMemberInfo>(json);
            if (model != null)
            {
                foreach(LimitedAmount list in model._LimitedAmounts)
                {
                    ServiceHelper.Create<IOrganizationService>().UpdateLimitedAmount(list);
                }
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult DeleteOrganization(long Id)
        {
            bool flag = ServiceHelper.Create<IOrganizationService>().DeleteOrganization(Id);
            if (flag)
                return Json(new { success = true,msg = "删除成功" });
            else
                return Json(new { success = false, msg = "删除失败" });
        }
        [HttpPost]
        public JsonResult IsExitsOrganization(long Id)
        {
            Organization Org = ServiceHelper.Create<IOrganizationService>().GetOrganizationById(Id);
            bool flag = ServiceHelper.Create<IOrganizationService>().IsExitsOrganization(Org.UserId);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult BatchDeleteOrganization(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            for (int i = 0; i < strArrays.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays[i]));
            }
            bool flag = ServiceHelper.Create<IOrganizationService>().BatchDeleteOrganization(nums.ToArray());
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult GetMember(long userId)
        {
            IQueryable<QueryMember> managerInfos = ServiceHelper.Create<IOrganizationService>().GetAllPermissionGroup(userId);
            if (managerInfos != null)
                return Json(new { success = true, data = managerInfos });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult GetAllMember(long roleId)
        {
            List<OrgMemberId> orglists = new List<OrgMemberId>();
            List<Organization> managerInfos = ServiceHelper.Create<IOrganizationService>().GetAllPerson(roleId);
            foreach (Organization list in managerInfos)
            {
                UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMember(list.UserId);
                OrgMemberId Org = new OrgMemberId();
                Org.Id = list.UserId;
                Org.Username = userInfo.UserName;
                orglists.Add(Org);
            }
            if (orglists != null)
                return Json(new { success = true, data = orglists });
            else
                return Json(new { success = false });
        }
    }
}