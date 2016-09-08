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
    public class PermissionGroupController : BaseWebController
    {
        
        public ActionResult Management()
        {
            ViewBag.UserId = base.CurrentUser.Id;
            return View();
        }

        [HttpPost]
        public JsonResult List(long Id, int page, int rows)
        {
            PageModel<PurchaseRolesInfo> roleInfo = ServiceHelper.Create<IPermissionGroupService>().GetPurchaseRoles(Id, page, rows);
            IEnumerable<PurchaseRolesInfo> array =
                from item in roleInfo.Models.ToArray()
                select new PurchaseRolesInfo()
                {
                    Id = item.Id,
                    MasterId = item.MasterId,
                    RoleName = item.RoleName
                };
            DataGridModel<PurchaseRolesInfo> dataGridModel = new DataGridModel<PurchaseRolesInfo>()
            {
                rows = array,
                total = roleInfo.Total
            };
            return Json(dataGridModel);
        }
        public ActionResult Add()
        {
            ViewBag.CoinDics = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(1);
            return View();
        }
        public ActionResult Edit(long Id)
        {
            PurchaseRolesInfo roleInfo = ServiceHelper.Create<IPermissionGroupService>().GetPurchaseRole(Id);
            ViewBag.Id = Id;
            ViewBag.RoleName = roleInfo.RoleName;
            ViewBag.Limited = ServiceHelper.Create<IOrganizationService>().GetLimitedAmountByRoleId(Id);
            ViewBag.CoinDics = ServiceHelper.Create<IChemCloud_DictionariesService>().GetListByType(1);
            return View();
        }
        [HttpPost]
        public JsonResult AddPermissionGroup(string RoleName)
        {
            PurchaseRolesInfo roleInfo = ServiceHelper.Create<IPermissionGroupService>().AddPermissionGroup(base.CurrentUser.Id, RoleName);
            if (roleInfo != null)
                return Json(new { success = true , roleId = roleInfo.Id });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult AddLimitedAmount(string json)
        {
            List<LimitedAmount> Limiteds = new List<LimitedAmount>();
            UserMemberInfo model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserMemberInfo>(json);
            if (model != null)
            {
                foreach (var item in model._LimitedAmounts)
                {
                    item.RoleId = item.RoleId;
                    item.Money = item.Money;
                    item.CoinType = item.CoinType;
                }
            }
            Limiteds = model._LimitedAmounts;
            bool flags = ServiceHelper.Create<IOrganizationService>().AddLimited(Limiteds);
            if (flags)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdatePermissionGroup(long Id, string rolename)
        {
            bool flag = ServiceHelper.Create<IPermissionGroupService>().UpdatePermissionGroup(Id, rolename);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult UpdateLimitedAmount(string json)
        {
            List<bool> flags = new List<bool>();
            List<LimitedAmount> Limiteds = new List<LimitedAmount>();
            UserMemberInfo model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserMemberInfo>(json);
            if (model != null)
            {
                foreach (LimitedAmount list in model._LimitedAmounts)
                {
                    ServiceHelper.Create<IOrganizationService>().UpdateLimitedAmount(list);
                }
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult Delete(long Id)
        {
            List<LimitedAmount> limited = ServiceHelper.Create<IOrganizationService>().GetLimitedAmountByRoleId(Id);
            if (limited != null)
            {
                List<long> roleIds = new List<long>();
                foreach (LimitedAmount list in limited)
                {
                    roleIds.Add(list.Id);
                }
                bool flag = ServiceHelper.Create<IPermissionGroupService>().Delete(Id, roleIds.ToArray());
                if (flag)
                    return Json(new { success = true, msg = "删除成功" });
                else
                    return Json(new { success = false, msg = "删除失败" });
            }
            else
                return Json(new { success = false, msg = "未能获取限额" });
        }
        [HttpPost]
        public JsonResult BatchDelete(string ids)
        {
            string[] strArrays = ids.Split(new char[] { ',' });
            List<long> nums = new List<long>();
            for (int i = 0; i < strArrays.Length; i++)
            {
                nums.Add(Convert.ToInt64(strArrays[i]));
            }
            List<long> roleIds = nums;
            bool flag = ServiceHelper.Create<IPermissionGroupService>().BatchDelete(nums.ToArray(),roleIds.ToArray());
            if (flag)
                return Json(new { success = true ,msg = "删除成功"});
            else
                return Json(new { success = false, msg = "删除失败" });
        }
        [HttpPost]
        public JsonResult IsExitsAdmin(long Id)
        {
            PurchaseRolesInfo roleInfo = ServiceHelper.Create<IPermissionGroupService>().GetPurchaseRole(Id);
            if(roleInfo.RoleName == "管理员" || roleInfo.RoleName == "Admin")
                return Json(new { success = true, msg = "删除成功" });
            else
                return Json(new { success = false, msg = "删除失败" }); 
        }
    }
}