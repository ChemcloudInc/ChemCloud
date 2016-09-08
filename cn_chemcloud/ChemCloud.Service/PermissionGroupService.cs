using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChemCloud.Service
{
    public class PermissionGroupService : ServiceBase, IPermissionGroupService, IService, IDisposable
    {
        /// <summary>
        /// 根据ID获取权限组
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PurchaseRolesInfo GetPurchaseRole(long Id)
        {
            PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Id);
            return roleInfo;
        }
        
        public List<PurchaseRolesInfo> GetPurchaseRolesByParentId(long ParentId)
        {
            IQueryable<PurchaseRolesInfo> roleInfos = from item in context.PurchaseRolesInfo
                                                      where item.MasterId == ParentId
                                                      select item;
            List<PurchaseRolesInfo> roleInfoLists = roleInfos.ToList();
            return roleInfoLists;
        }

        public PageModel<PurchaseRolesInfo> GetPurchaseRoles(long MasterId,int page, int rows)
        {
            IQueryable<PurchaseRolesInfo> roleInfos = from item in base.context.PurchaseRolesInfo
                                                      where item.MasterId == MasterId
                                                      select item;
            Func<IQueryable<PurchaseRolesInfo>, IOrderedQueryable<PurchaseRolesInfo>> func = null;
            func = (IQueryable<PurchaseRolesInfo> d) =>
                    from o in d
                    orderby o.Id descending
                    select o;
            int num = roleInfos.Count();
            roleInfos = roleInfos.GetPage(out num, page, rows, func);
            return new PageModel<PurchaseRolesInfo>()
            {
                Models = roleInfos,
                Total = num
            };
        }
        public PurchaseRolesInfo AddPermissionGroup(long parentId,string rolename)
        {
            PurchaseRolesInfo roleInfos ;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                PurchaseRolesInfo roleInfo = new PurchaseRolesInfo()
                {
                    MasterId = parentId,
                    RoleName = rolename
                };
                roleInfos = roleInfo;
                roleInfos = context.PurchaseRolesInfo.Add(roleInfos);
                context.SaveChanges();
                transactionScope.Complete();
            }
            return roleInfos;
        }
        public bool UpdatePermissionGroup(long Id, string RoleName)
        {
            PurchaseRolesInfo roleInfo = GetPurchaseRole(Id);
            if (roleInfo.RoleName != RoleName)
            {
                roleInfo.RoleName = RoleName;
                int i = context.SaveChanges();
                if (i > 0)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }
        public bool Delete(long Id,long[] Ids)
        {
            int i = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                PurchaseRolesInfo roleInfo = GetPurchaseRole(Id);
                context.PurchaseRolesInfo.Remove(roleInfo);
                IQueryable<LimitedAmount> limiteds = context.LimitedAmount.FindBy((LimitedAmount item) => Ids.Contains(item.RoleId));
                context.LimitedAmount.RemoveRange(limiteds);
                i = context.SaveChanges();
                transactionScope.Complete();
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        
        public bool BatchDelete(long[] Ids,long[] RoleIds)
        {
            int i = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                IQueryable<PurchaseRolesInfo> roleInfos = context.PurchaseRolesInfo.FindBy((PurchaseRolesInfo item) => Ids.Contains(item.Id));
                context.PurchaseRolesInfo.RemoveRange(roleInfos);
                IQueryable<LimitedAmount> limiteds = context.LimitedAmount.FindBy((LimitedAmount item) => RoleIds.Contains(item.RoleId));
                context.LimitedAmount.RemoveRange(limiteds);
                i = context.SaveChanges();
                transactionScope.Complete();
            }
            if (i > 0)
                return true;
            else
                return false;
            
        }
        public PurchaseRolesInfo GetPurchaseRoleByMasterId(long MasterId, string RoleName)
        {
            PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.MasterId == MasterId && m.RoleName == RoleName);
            return roleInfo;
        }
        public PurchaseRolesInfo GetPurchaseRoleByUserId(long MasterId)
        {
            PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m)=>m.MasterId == MasterId);
            return roleInfo;
        }
    }
}
