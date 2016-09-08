using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Areas.Admin.Models;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.SellerAdmin.Controllers
{
    //public class ManagerController : BaseSellerController
    //{
    //    public ManagerController()
    //    {
    //    }

    //    [ShopOperationLog(Message = "添加供应商子帐号")]
    //    public JsonResult Add(ManagerInfoModel model, string email, string roleJson)
    //    {
    //        Result result = new Result();
    //        long RoleId = 0;
    //        bool flags = false;
    //        string userName = base.CurrentSellerManager.UserName;
    //        char[] chrArray = new char[] { ':' };
    //        string str = userName.Split(chrArray)[0];
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        long CertificationId = base.CurrentSellerManager.CertificationId;
    //        string str1 = string.Concat(str, ":", model.UserName);
    //        UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(userName);
    //        ManagerInfo managerInfo = new ManagerInfo()
    //        {
    //            UserName = str1,
    //            Password = model.Password,
    //            RoleId = 0,
    //            ShopId = shopId,
    //            Remark = model.Remark,
    //            RealName = model.RealName,
    //            CertificationId = CertificationId
    //        };
    //        //bool Manager = ServiceHelper.Create<IManagerService>().AddSellerManager(managerInfo, str1);
    //        UserMemberInfo memberInfo = new UserMemberInfo()
    //        {
    //            UserName = str1,
    //            Password = model.Password,
    //            Email = email,
    //            RealName = model.RealName,
    //            Nick = str1,
    //            ParentSellerId = userInfo.Id,
    //            Disabled = true,
    //            LastLoginDate = DateTime.Now,
    //            UserType = 2
    //        };
    //        //bool Member =  ServiceHelper.Create<IMemberService>().AddSellerChild(memberInfo);
    //        if (!base.ModelState.IsValid)
    //        {
    //            return Json(new { success = true, msg = "验证失败" });
    //        }
    //        JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
    //        {
    //            MissingMemberHandling = MissingMemberHandling.Ignore,
    //            NullValueHandling = NullValueHandling.Ignore
    //        };
    //         RoleInfo shopIds = JsonConvert.DeserializeObject<RoleInfo>(roleJson, jsonSerializerSetting);
    //        shopIds.ShopId = base.CurrentSellerManager.ShopId;
    //        bool flag = ServiceHelper.Create<IPrivilegesService>().AddChileRole(managerInfo, memberInfo, shopIds);
    //        if (flag)
    //        {
    //            RoleId = ServiceHelper.Create<IPrivilegesService>().GetRoleId(model.UserName, shopIds.ShopId);
    //            flags = ServiceHelper.Create<IManagerService>().UpdateSellerManager(str1, shopIds.ShopId, RoleId);
    //        }
    //        if (flags)
    //        {
    //            result.success = true;
    //            result.msg = "添加成功";
    //        }
    //        else
    //        {
    //            result.success = false;
    //            result.msg = "添加失败";
    //        }
    //        return Json(result);
    //    }

    //    [HttpPost]
    //    [ShopOperationLog(Message = "批量删除管理员")]
    //    public JsonResult BatchDelete(string ids)
    //    {
    //        bool flag = false;
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        string username = base.CurrentSellerManager.UserName;
    //        string[] strArrays = ids.Split(new char[] { ',' });
    //        List<long> nums = new List<long>();
    //        List<long> roleIdList = new List<long>();
    //        List<long> memberIdList = new List<long>();
    //        for (int i = 0; i < strArrays.Length; i++)
    //        {
    //            nums.Add(Convert.ToInt64(strArrays[i]));
    //        }

    //        foreach (long Id in nums)
    //        {
    //            ManagerInfo roleInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(Id);
    //            if (roleInfo != null)
    //            {
    //                UserMemberInfo memberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(roleInfo.UserName);
    //                memberIdList.Add(memberInfo.Id);
    //                roleIdList.Add(roleInfo.RoleId);
    //            }
    //        }
    //        if (memberIdList.Count > 0 && roleIdList.Count > 0)
    //        {
    //            flag = ServiceHelper.Create<IPrivilegesService>().BatchDeleteSellerRole(nums.ToArray(), memberIdList.ToArray(), roleIdList.ToArray(), shopId);
    //        }
    //        Result result = new Result();
    //        if (flag)
    //        {
    //            result.success = true;
    //            result.msg = "批量删除成功！";
    //        }
    //        else
    //        {
    //            result.success = false;
    //            result.msg = "批量删除失败！";
    //        }
    //        return Json(result);
    //    }

    //    [ShopOperationLog(Message = "修改供应商管理员")]
    //    public JsonResult Change(long id, string password, long roleId, string realName, string reMark)
    //    {
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        long CertificationId = base.CurrentSellerManager.CertificationId;
    //        ManagerInfo managerInfo = new ManagerInfo()
    //        {
    //            Id = id,
    //            Password = password,
    //            RoleId = roleId,
    //            RealName = realName,
    //            Remark = reMark,
    //            ShopId = shopId,
    //            CertificationId = CertificationId
    //        };
    //        ServiceHelper.Create<IManagerService>().ChangeSellerManager(managerInfo);
    //        Result result = new Result()
    //        {
    //            success = true,
    //            msg = "修改成功！"
    //        };
    //        return Json(result);
    //    }

    //    [HttpPost]
    //    [ShopOperationLog(Message = "删除供应商子帐号")]
    //    public JsonResult Delete(long id)
    //    {
    //        bool flag = false;
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        string username = base.CurrentSellerManager.UserName;
    //        ManagerInfo roleInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(id);
    //        if (roleInfo != null)
    //        {
    //            UserMemberInfo memberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(roleInfo.UserName);
    //            long RoleId = roleInfo.RoleId;
    //            if (base.CurrentSellerManager.Id == id)
    //            {
    //                Result result = new Result()
    //                {
    //                    success = false,
    //                    msg = "不能删除自身！"
    //                };
    //                return Json(result);
    //            }
    //            flag = ServiceHelper.Create<IPrivilegesService>().DeleteSellerRole(RoleId, shopId, memberInfo.Id, id);
    //        }
    //        Result result1 = new Result();
    //        if (flag)
    //        {
    //            result1.success = true;
    //            result1.msg = "删除成功！";
    //        }
    //        else
    //        {
    //            result1.success = false;
    //            result1.msg = "删除失败！";
    //        }
    //        return Json(result1);
    //    }

    //    [UnAuthorize]
    //    public JsonResult IsExistsUserName(string userName)
    //    {
    //        string str = base.CurrentSellerManager.UserName;
    //        char[] chrArray = new char[] { ':' };
    //        string str1 = str.Split(chrArray)[0];
    //        userName = string.Concat(str1, ":", userName);
    //        bool flag = ServiceHelper.Create<IManagerService>().CheckUserNameExist(userName, false);
    //        return Json(new { success = true, result = flag });
    //    }

    //    public JsonResult List(int page, string keywords, int rows, bool? status = null)
    //    {
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        long id = base.CurrentSellerManager.Id;
    //        IManagerService managerService = ServiceHelper.Create<IManagerService>();
    //        ManagerQuery managerQuery = new ManagerQuery()
    //        {
    //            PageNo = page,
    //            PageSize = rows,
    //            ShopID = shopId,
    //            userID = id
    //        };
    //        PageModel<ManagerInfo> sellerManagers = managerService.GetSellerManagers(managerQuery);
    //        List<RoleInfo> list = ServiceHelper.Create<IPrivilegesService>().GetSellerRoles(shopId).ToList();
    //        var collection =
    //            from item in sellerManagers.Models.ToList()
    //            select new
    //            {
    //                Id = item.Id,
    //                UserName = item.UserName,
    //                CreateDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm"),
    //                RoleName = (
    //                    from a in list
    //                    where a.Id == item.RoleId
    //                    select a).FirstOrDefault().RoleName,
    //                RoleId = item.RoleId,
    //                realName = item.RealName,
    //                reMark = item.Remark
    //            };
    //        return Json(new { rows = collection, total = sellerManagers.Total });
    //    }

    //    public ActionResult Management()
    //    {
    //        string userName = base.CurrentSellerManager.UserName;
    //        dynamic viewBag = base.ViewBag;
    //        char[] chrArray = new char[] { ':' };
    //        viewBag.MainUserName = userName.Split(chrArray)[0];
    //        ViewBag.UserId = base.CurrentSellerManager.Id;
    //        ViewBag.ShopId = base.CurrentSellerManager.ShopId;
    //        SetPrivileges();
    //        return View();
    //    }
    //    [HttpGet]
    //    public ActionResult Edit(long id)
    //    {
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        SetPrivileges();
    //        ManagerInfo sellerInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(id);
    //        long roleId = sellerInfo.RoleId;
    //        RoleInfo sellerRole = ServiceHelper.Create<IPrivilegesService>().GetSellerRole(roleId, shopId);
    //        RoleInfoModel roleInfoModel = new RoleInfoModel()
    //        {
    //            ID = sellerRole.Id,
    //            RoleName = sellerInfo.RoleName,
    //            UserName = sellerInfo.UserName,
    //            Description = sellerRole.Description
    //        };
    //        RoleInfoModel roleInfoModel1 = roleInfoModel;
    //        JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
    //        {
    //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //        };
    //        dynamic viewBag = base.ViewBag;
    //        ICollection<RolePrivilegeInfo> rolePrivilegeInfo = sellerRole.RolePrivilegeInfo;
    //        viewBag.RolePrivilegeInfo = JsonConvert.SerializeObject(
    //            from item in rolePrivilegeInfo
    //            select new { Privilege = item.Privilege }, jsonSerializerSetting);
    //        viewBag.ShopId = shopId;
    //        viewBag.roleId = roleId;
    //        viewBag.roleName = sellerInfo.RoleName;
    //        return View(roleInfoModel1);
    //    }
    //    [HttpPost]
    //    [UnAuthorize]
    //    public JsonResult Edit(string roleJson, long id)
    //    {
    //        if (!base.ModelState.IsValid)
    //        {
    //            return Json(new { success = true, msg = "验证失败" });
    //        }
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
    //        {
    //            MissingMemberHandling = MissingMemberHandling.Ignore,
    //            NullValueHandling = NullValueHandling.Ignore
    //        };
    //        RoleInfo roleInfo = JsonConvert.DeserializeObject<RoleInfo>(roleJson, jsonSerializerSetting);
    //        roleInfo.Id = id;
    //        roleInfo.ShopId = base.CurrentSellerManager.ShopId;
    //        ServiceHelper.Create<IPrivilegesService>().UpdateSellerRole(roleInfo);
    //        List<ManagerInfo> list = ServiceHelper.Create<IManagerService>().GetSellerManagerByRoleId(id, shopId).ToList();
    //        foreach (ManagerInfo managerInfo in list)
    //        {
    //            Cache.Remove(CacheKeyCollection.Seller(managerInfo.Id));
    //        }
    //        return Json(new { success = true });
    //    }
    //    [HttpPost]
    //    public JsonResult CheckRoleName(string username, long shopId)
    //    {
    //        bool flag = ServiceHelper.Create<IPrivilegesService>().CheckRoleNameExist(username, shopId);
    //        return Json(new { success = flag, result = flag });
    //    }
    //    [HttpPost]
    //    public JsonResult CheckEmail(string email)
    //    {
    //        bool flag = ServiceHelper.Create<IMemberService>().CheckEmailExist(email);
    //        return Json(new { success = flag, result = flag });
    //    }
    //    [HttpPost]
    //    public JsonResult RoleList()
    //    {
    //        long shopId = base.CurrentSellerManager.ShopId;
    //        var sellerRoles =
    //            from item in ServiceHelper.Create<IPrivilegesService>().GetSellerRoles(shopId)
    //            select new { Id = item.Id, RoleName = item.RoleName };
    //        return Json(sellerRoles);
    //    }
    //    private void SetPrivileges()
    //    {
    //        ViewBag.Privileges = PrivilegeHelper.SellerAdminPrivileges;
    //    }
    //}
    public class ManagerController : BaseSellerController
    {
        public ManagerController()
        {
        }

        [ShopOperationLog(Message = "添加卖家子帐号")]
        public JsonResult Add(ManagerInfoModel model,string email)
        {
            Result result = new Result();
            string userName = base.CurrentSellerManager.UserName;
            char[] chrArray = new char[] { ':' };
            string str = userName.Split(chrArray)[0];
            //long userId = base.CurrentSellerManager.Id;
            long shopId = base.CurrentSellerManager.ShopId;
            long CertificationId = base.CurrentSellerManager.CertificationId;
            string str1 = string.Concat(str, ":", model.UserName);
            UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(userName);
            if(userInfo != null)
            {
                ManagerInfo managerInfo = new ManagerInfo()
                {
                    UserName = str1,
                    Password = model.Password,
                    RoleId = model.RoleId,
                    ShopId = shopId,
                    Remark = model.Remark,
                    RealName = model.RealName,
                    CertificationId = CertificationId
                };
                UserMemberInfo memberInfo = new UserMemberInfo()
                {
                    UserName = str1,
                    Password = model.Password,
                    Email = email,
                    RealName = model.RealName,
                    Nick = str1,
                    ParentSellerId = userInfo.Id,
                    Disabled = true,
                    LastLoginDate = DateTime.Now,
                    UserType = 2
                };
                bool flag = ServiceHelper.Create<IPrivilegesService>().AddChileRole(managerInfo, memberInfo);
                if (flag)
                {
                    result.success = true;
                    result.msg = "添加成功";
                }
                else
                {
                    result.success = false;
                    result.msg = "添加失败";
                }
            }
            else
            {
                result.success = false;
                result.msg = "添加失败";
            }
            return Json(result);
        }

        [HttpPost]
        [ShopOperationLog(Message = "批量删除管理员")]
        public JsonResult BatchDelete(string ids)
        {
            bool flag = false;
            long shopId = base.CurrentSellerManager.ShopId;
            string username = base.CurrentSellerManager.UserName;
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            List<long> memberIdList = new List<long>();
            for (int i = 0; i < strArrays.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays[i]));
            }

            foreach (long Id in nums)
            {
                ManagerInfo roleInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(Id);
                if (roleInfo != null)
                {
                    UserMemberInfo memberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(username);
                    memberIdList.Add(memberInfo.Id);
                }
            }
            if (memberIdList.Count > 0 )
            {
                flag = ServiceHelper.Create<IPrivilegesService>().BatchDeleteSellerRole(nums.ToArray(), memberIdList.ToArray(),shopId);
            }
            Result result = new Result();
            if (flag)
            {
                result.success = true;
                result.msg = "批量删除成功！";
            }
            else
            {
                result.success = false;
                result.msg = "批量删除失败！";
            }
            return Json(result);
        }
        [HttpPost]
        [ShopOperationLog(Message = "修改商家管理员")]
        public JsonResult Change(long id,string email, long roleId, string realName, string reMark)
        {
            Result result = new Result();
            long shopId = base.CurrentSellerManager.ShopId;
            ManagerInfo manaInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(id);
            if(manaInfo != null)
            {
                string UserName = manaInfo.UserName;
                UserMemberInfo userInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(UserName);
                ManagerInfo managerInfo = new ManagerInfo()
                {
                    Id = id,
                    RoleId = roleId,
                    RealName = realName,
                    Remark = reMark,
                    ShopId = shopId
                };
                UserMemberInfo memberInfo = new UserMemberInfo()
                {
                    Id = userInfo.Id,
                    RealName = realName,
                    Email = email,
                    Remark = reMark
                };
                bool flag =  ServiceHelper.Create<IManagerService>().ChangeSellerManager(managerInfo,memberInfo);
                if (flag)
                {
                    result.success = true;
                    result.msg = "修改成功";
                }
                else
                {
                    result.success = false;
                    result.msg = "修改失败";
                }
            }
            else
            {
                result.success = false;
                result.msg = "修改失败";
            }
            return Json(result);
        }
        
        [HttpPost]
        [ShopOperationLog(Message = "删除卖家子帐号")]
        public JsonResult Delete(long id)
        {
            bool flag = false;
            long shopId = base.CurrentSellerManager.ShopId;
            if (base.CurrentSellerManager.RoleId == 0)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "不能删除管理员！"
                };
                return Json(result);
            }
            if (base.CurrentSellerManager.Id == id)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "不能删除自身！"
                };
                return Json(result);
            }
            
            ManagerInfo roleInfo = ServiceHelper.Create<IManagerService>().GetSellerManager(id);
            Result result1 = new Result();
            if(roleInfo != null)
            {
                string username = roleInfo.UserName;
                if (roleInfo != null)
                {
                    UserMemberInfo memberInfo = ServiceHelper.Create<IMemberService>().GetMemberByName(username);
                    flag = ServiceHelper.Create<IPrivilegesService>().DeleteSellerRoles(shopId, memberInfo.Id, id);
                }
                if (flag)
                {
                    result1.success = true;
                    result1.msg = "删除成功！";
                }
                else
                {
                    result1.success = false;
                    result1.msg = "删除失败！";
                }
            }
            else
            {
                result1.success = false;
                result1.msg = "无法查询到该用户请查！";
            }
            return Json(result1);
        }

        [UnAuthorize]
        public JsonResult IsExistsUserName(string userName)
        {
            string str = base.CurrentSellerManager.UserName;
            char[] chrArray = new char[] { ':' };
            string str1 = str.Split(chrArray)[0];
            userName = string.Concat(str1, ":", userName);
            bool flag = ServiceHelper.Create<IManagerService>().CheckUserNameExist(userName, false);
            return Json(new { success = true, result = flag });
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool flag = ServiceHelper.Create<IMemberService>().CheckEmailExist(email);
            return Json(new { success = flag, result = flag });
        }
        public JsonResult List(int page, string keywords, int rows, bool? status = null)
        {
            long shopId = base.CurrentSellerManager.ShopId;
            long id = base.CurrentSellerManager.Id;
            long roleId = base.CurrentSellerManager.RoleId;
            IManagerService managerService = ServiceHelper.Create<IManagerService>();
            ManagerQuery managerQuery = new ManagerQuery()
            {
                PageNo = page,
                PageSize = rows,
                ShopID = shopId,
                userID = id,
                roleID = roleId
            };
            PageModel<ManagerInfo> sellerManagers = managerService.GetSellerManagers(managerQuery);

            //List<RoleInfo> list = ServiceHelper.Create<IPrivilegesService>().GetSellerRoles(shopId).ToList();
            var collection =
                from item in sellerManagers.Models.ToList()
                select new
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    CreateDate = item.CreateDate.ToString("yyyy-MM-dd HH:mm"),
                    RoleName = item.RoleName,
                    RoleId = item.RoleId,
                    realName = item.RealName,
                    Emails = item.Email,
                    reMark = item.Remark
                };
            return Json(new { rows = collection, total = sellerManagers.Total });
        }

        public ActionResult Management()
        {
            string userName = base.CurrentSellerManager.UserName;
            dynamic viewBag = base.ViewBag;
            char[] chrArray = new char[] { ':' };
            viewBag.MainUserName = userName.Split(chrArray)[0];
            ViewBag.UserId = base.CurrentSellerManager.Id;
            return View();
        }

        [HttpPost]
        public JsonResult RoleList()
        {
            long shopId = base.CurrentSellerManager.ShopId;
            var sellerRoles =
                from item in ServiceHelper.Create<IPrivilegesService>().GetSellerRoles(shopId)
                select new { Id = item.Id, RoleName = item.RoleName };
            return Json(sellerRoles);
        }
    }
}