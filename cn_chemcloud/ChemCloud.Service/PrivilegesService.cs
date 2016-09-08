using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace ChemCloud.Service
{
    public class PrivilegesService : ServiceBase, IPrivilegesService, IService, IDisposable
    {
        public PrivilegesService()
        {
        }
        public bool AddChileRole(ManagerInfo managerInfo, UserMemberInfo userInfo)
        {
            int i = 0;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                managerInfo.PasswordSalt = Guid.NewGuid().ToString();
                managerInfo.CreateDate = DateTime.Now;
                string str = SecureHelper.MD5(managerInfo.Password);
                managerInfo.Password = SecureHelper.MD5(string.Concat(str, managerInfo.PasswordSalt));
                context.ManagerInfo.Add(managerInfo);
                userInfo.PasswordSalt = managerInfo.PasswordSalt;
                userInfo.CreateDate = DateTime.Now;
                userInfo.Password = SecureHelper.MD5(string.Concat(str, userInfo.PasswordSalt));
                context.UserMemberInfo.Add(userInfo);
                i = context.SaveChanges();
                transactionScope.Complete();
            }
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool AddPlatformRole(RoleInfo model)
        {
            model.ShopId = 0;
            if (string.IsNullOrEmpty(model.Description))
            {
                model.Description = model.RoleName;
            }
            context.RoleInfo.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }

        public void AddSellerRole(RoleInfo model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                model.Description = model.RoleName;
            }
            context.RoleInfo.Add(model);
            context.SaveChanges();
        }
        public bool BatchDeletPlatformRole(long[] ids, long[] roleIds)
        {
            int i = 0;
            using(TransactionScope transactionScope = new TransactionScope())
            {
                IQueryable<ManagerInfo> managerInfos = context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == 0 && item.RoleId != 0 && ids.Contains(item.Id));
                context.ManagerInfo.RemoveRange(managerInfos);
                IQueryable<RoleInfo> roleInfos = context.RoleInfo.FindBy((RoleInfo item) => item.ShopId == 0 && roleIds.Contains(item.Id));
                context.ManagerInfo.RemoveRange(managerInfos);
                i = context.SaveChanges();
                transactionScope.Complete();
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool DeletePlatformRole(long id)
        {
            RoleInfo roleInfo = (
                from a in context.RoleInfo
                where a.Id == id && a.ShopId == 0
                select a).FirstOrDefault();
            context.RoleInfo.Remove(roleInfo);
            int i =  context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool DeletePlatformRoles(long id, long roleId)
        {
            int i = 0;
            using(TransactionScope transactionScope = new TransactionScope())
            {
                ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0 && item.RoleId != 0).FirstOrDefault();
                RoleInfo roleInfo = (
                    from a in context.RoleInfo
                    where a.Id == roleId && a.ShopId == 0
                    select a).FirstOrDefault();
                if ((managerInfo != null) && (roleInfo != null))
                {
                    context.ManagerInfo.Remove(managerInfo);
                    context.RoleInfo.Remove(roleInfo);
                    i = context.SaveChanges();
                    transactionScope.Complete();
                }
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool BatchDeleteSellerRole(long[] ids, long[] memberIds, long shopId)
        {
            int i = 0;
            using(TransactionScope transactionScope = new TransactionScope())
            {
                IQueryable<ManagerInfo> managerInfos = context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == shopId && item.RoleId != 0 && ids.Contains(item.Id));
                context.ManagerInfo.RemoveRange(managerInfos);
                IQueryable<UserMemberInfo> userInfos = context.UserMemberInfo.FindBy((UserMemberInfo item) => memberIds.Contains(item.Id));
                context.UserMemberInfo.RemoveRange(userInfos);
                i = context.SaveChanges();
                transactionScope.Complete();
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool DeleteSellerRoles(long shopId, long memberId, long userId)
        {
            int i = 0;
            using(TransactionScope transactionScope = new TransactionScope())
            {
                ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == userId && item.ShopId == shopId && item.RoleId != 0).FirstOrDefault();
                UserMemberInfo userInfo = context.UserMemberInfo.FindBy((UserMemberInfo item) => item.Id == memberId).FirstOrDefault();
                if (managerInfo != null && userInfo != null)
                {
                    try
                    {
                        context.ManagerInfo.Remove(managerInfo);
                        context.UserMemberInfo.Remove(userInfo);
                        i = context.SaveChanges();
                        transactionScope.Complete();
                    }
                    catch (DbEntityValidationException dbEx)
                    {

                    }
                    
                }
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public void DeleteSellerRole(long id, long shopId)
        {
            RoleInfo roleInfo = (
                from a in context.RoleInfo
                where a.Id == id && a.ShopId == shopId
                select a).FirstOrDefault();
            context.RoleInfo.Remove(roleInfo);
            context.SaveChanges();
        }
        public long GetPlatformRoleId(string roleName)
        {
            RoleInfo roleInfo = context.RoleInfo.FirstOrDefault((RoleInfo m) => m.RoleName == roleName && m.ShopId == 0);
            long roleId = roleInfo.Id;
            return roleId;
        }
        public RoleInfo GetPlatformRole(long id)
        {
            return (
                from a in context.RoleInfo
                where a.Id == id && a.ShopId == 0
                select a).FirstOrDefault();
        }

        public IQueryable<RoleInfo> GetPlatformRoles()
        {
            return context.RoleInfo.FindBy((RoleInfo item) => item.ShopId == 0);
        }
        public bool CheckRoleNameExist(string username, long shopId)
        {
            RoleInfo roleInfo = context.RoleInfo.FirstOrDefault((RoleInfo item) => item.RoleName.ToLower().Equals(username.ToLower()) && (item.ShopId == shopId));
            if (roleInfo != null)
                return true;
            else
                return false;
            //return context.RoleInfo.Any((RoleInfo item) => (item.RoleName.ToLower() == username.ToLower()) && item.ShopId == shopId);
        }
        public RoleInfo GetSellerRole(long id, long shopid)
        {
            return (
                from a in context.RoleInfo
                where a.Id == id && a.ShopId == shopid
                select a).FirstOrDefault();
        }
        public long GetRoleId(string RoleName, long ShopId)
        {
            RoleInfo roleInfo = context.RoleInfo.FirstOrDefault((RoleInfo r) => (r.RoleName.Equals(RoleName) && (r.ShopId.Equals(ShopId))));
            long roleId = 0;
            if (roleInfo != null)
            {
                roleId = roleInfo.Id;
            }
            return roleId;
        }
        public IQueryable<RoleInfo> GetSellerRoles(long shopId)
        {
            return
                from item in context.RoleInfo
                where item.ShopId == shopId && item.ShopId != 0
                select item;
        }

        public void UpdatePlatformRole(RoleInfo model)
        {
            RoleInfo roleName = context.RoleInfo.FindBy((RoleInfo a) => a.ShopId == 0 && a.Id == model.Id).FirstOrDefault();
            if (roleName == null)
            {
                throw new HimallException("找不到该权限组");
            }                       
            roleName.RoleName = model.RoleName;
            roleName.Description = model.Description;
            if (string.IsNullOrEmpty(model.Description))
            {
                roleName.Description = model.RoleName;
            }
            context.RolePrivilegeInfo.RemoveRange(roleName.RolePrivilegeInfo);
            roleName.RolePrivilegeInfo = model.RolePrivilegeInfo;
            context.SaveChanges();
        }

        public void UpdateSellerRole(RoleInfo model)
        {
            RoleInfo roleName = context.RoleInfo.FindBy((RoleInfo a) => a.ShopId == model.ShopId && a.Id == model.Id).FirstOrDefault();
            if (roleName == null)
            {
                throw new HimallException("找不到该权限组");
            }
            roleName.RoleName = model.RoleName;
            roleName.Description = model.Description;
            if (string.IsNullOrEmpty(model.Description))
            {
                roleName.Description = model.RoleName;
            }
            context.RolePrivilegeInfo.RemoveRange(roleName.RolePrivilegeInfo);
            roleName.RolePrivilegeInfo = model.RolePrivilegeInfo;
            context.SaveChanges();
        }


        public bool HasMessage(long roleId)
        {
            return (from a in context.RolePrivilegeInfo where a.RoleId == roleId && (a.Privilege == 0 || a.Privilege == 6004) select a).FirstOrDefault<RolePrivilegeInfo>() != null;
        }
    }
}