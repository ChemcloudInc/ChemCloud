using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    
    public class ManagerController : BaseAdminController
    {
        public ManagerController()
        {
        }
        [HttpPost]
        public JsonResult Add(ManagerInfoModel model)
        {
            Result result = new Result();
            try
            {
                ManagerInfo managerInfo = new ManagerInfo()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    RoleId = model.RoleId
                };
                ServiceHelper.Create<IManagerService>().AddPlatformManager(managerInfo);
                result.success = true;
                result.msg = "添加成功！";
            }
            catch(Exception ex)
            {
                result.success = false;
                result.msg = ex.ToString();
            }
            return Json(result);
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult BatchDelete(string ids)
        {
            Result result = new Result();
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < strArrays1.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays1[i]));
            }
            try
            {
                ServiceHelper.Create<IManagerService>().BatchDeletePlatformManager(nums.ToArray());
                result.success = true;
                result.msg = "批量删除成功！";
            }
            catch (Exception ex)
            {
                result.success = false;
                result.msg = ex.ToString();
            }
            return Json(result);
        }
        [HttpPost]
        [UnAuthorize]
        public JsonResult ChangePassWord(long id, string password)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IManagerService>().ChangePlatformManagerPassword(id, password);
                result.success = true;
                result.msg = "修改成功！";
            }
            catch (Exception ex)
            {
                result.success = false;
                result.msg = ex.ToString();
            }
            return Json(result);
        }
        [HttpPost]
        [UnAuthorize]
        public JsonResult ChangeManager(long id, long roleid)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IManagerService>().ChangePlatformManager(id, roleid);
                result.success = true;
                result.msg = "修改成功！";
            }
            catch (Exception ex)
            {
                result.success = false;
                result.msg = ex.ToString();
            }
            return Json(result);
        }
        [HttpPost]
        [UnAuthorize]
        public JsonResult Delete(long id)
        {
            Result result = new Result();
            try
            {
                ServiceHelper.Create<IManagerService>().DeletePlatformManager(id);
                result.success = true;
                result.msg = "删除成功！";
            }
            catch (Exception ex)
            {
                result.success = false;
                result.msg = ex.ToString();
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult ResetPwd(long id)
        {
            string passWord = "123456";
            ManagerInfo manaInfo = ServiceHelper.Create<IManagerService>().GetPlatformManager(id);
            if (manaInfo == null)
            {
                return Json(new { success = false, flag = -1, msg = "验证超时" });
            }
            bool flag = ServiceHelper.Create<IMemberService>().ResetPwd(id, passWord);
            if (flag)
                return Json(new { success = true, msg = "成功重置密码" });
            else
                return Json(new { success = false, msg = "重置密码失败" });
        }
        [HttpPost]
        public JsonResult IsSelf(long id)
        {
            if (id == base.CurrentManager.Id)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult IsExistsUserName(string userName)
        {
            bool flag = ServiceHelper.Create<IManagerService>().CheckUserNameExist(userName, true);
            return Json(new { success = true, result = flag });
        }

        [UnAuthorize]
        public JsonResult List(int page, string keywords, int rows, bool? status = null)
        {
            IManagerService managerService = ServiceHelper.Create<IManagerService>();
            ManagerQuery managerQuery = new ManagerQuery()
            {
                PageNo = page,
                PageSize = rows
            };
            PageModel<ManagerInfo> platformManagers = managerService.GetPlatformManagers(managerQuery);
            List<RoleInfo> list = ServiceHelper.Create<IPrivilegesService>().GetPlatformRoles().ToList();
            var collection =
                from item in platformManagers.Models.ToList()
                select new
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    CreateDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    RoleName = (item.RoleId == 0 ? "系统管理员" : (
                        from a in list
                        where a.Id == item.RoleId
                        select a).FirstOrDefault().RoleName),
                    RoleId = item.RoleId
                };
            return Json(new { rows = collection, total = platformManagers.Total });
        }

        public ActionResult Management()
        {
            ViewBag.userName = base.CurrentManager.UserName;
            ViewBag.userRoleId = base.CurrentManager.RoleId;
            return View();
        }

        [HttpPost]
        [UnAuthorize]
        public JsonResult RoleList()
        {
            var platformRoles =
                from item in ServiceHelper.Create<IPrivilegesService>().GetPlatformRoles()
                select new { Id = item.Id, RoleName = item.RoleName };
            return Json(platformRoles);
        }
    }
}