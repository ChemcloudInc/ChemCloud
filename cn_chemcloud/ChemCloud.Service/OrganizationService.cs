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
    public class OrganizationService : ServiceBase, IOrganizationService, IService, IDisposable
    {
        public List<Organization> GetOrganizations(long Id)
        {
            List<Organization> OrgLists = new List<Organization>();
            IQueryable<Organization> Org = from item in context.Organization
                                           where item.Id == Id
                                           select item;
            
            foreach (Organization list in Org.ToList())
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.UserId);
                PurchaseRolesInfo UserInfos = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == list.RoleId);
                if (userInfo != null)
                {
                    list.UserName = userInfo.UserName;
                    list.RealName = userInfo.RealName;
                    
                }
                if (UserInfos != null)
                {
                    list.RoleName = UserInfos.RoleName;
                }
                OrgLists.Add(list);
            }
            return OrgLists;
        }
        public List<Organization>ChildOrganization(long Id)
        {
            List<Organization> OrgLists = new List<Organization>();
            Organization Orgs = (from item in context.Organization
                                 where item.Id == Id
                                 select item).FirstOrDefault();
            IQueryable<Organization> Org = from item in context.Organization
                                           where item.ParentId == Orgs.UserId
                                           select item;
            foreach (Organization list in Org.ToList())
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.UserId);
                PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == list.RoleId);
                if(roleInfo != null)
                {
                    PurchaseRolesInfo UserInfos = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == list.ParentRoleId);
                    if (userInfo != null)
                    {
                        list.UserName = userInfo.UserName;
                        list.RealName = userInfo.RealName;
                    }
                    if (UserInfos != null)
                    {
                        list.RoleName = roleInfo.RoleName;
                        list.ParentRoleName = UserInfos.RoleName;
                    }
                    OrgLists.Add(list);
                }
                
            }
            return OrgLists;
        }
        public bool AddOrganization(Organization Org)
        {
            context.Organization.Add(Org);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool IsAdmin(long Id)
        {
            Organization Org = context.Organization.FirstOrDefault((Organization m)=>m.Id==Id);
            if(Org != null)
            {
                if(Org.ParentRoleId == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool UpdateOrganization(Organization model)
        {
            Organization Orgs = context.Organization.FindById<Organization>(model.Id);
            Orgs.UserId = model.UserId;
            Orgs.RoleId = model.RoleId;
            Orgs.ParentRoleId = model.ParentRoleId;
            Orgs.ParentId = model.ParentId;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool UpdateLimitedAmount(LimitedAmount model)
        {
            LimitedAmount limited = context.LimitedAmount.FindById<LimitedAmount>(model.Id);
            limited.RoleId = model.RoleId;
            limited.Money = model.Money;
            limited.CoinType = model.CoinType;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool DeleteOrganization(long Id)
        {
            int i = 0;
            try
            {
                Organization Orgs = context.Organization.FindById<Organization>(Id);
                UserMemberInfo userInfo = new UserMemberInfo();
                if (Orgs != null)
                {
                    userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Orgs.UserId);
                }
                context.Organization.Remove(Orgs);
                context.UserMemberInfo.Remove(userInfo);
                i = context.SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool AddLimited(List<LimitedAmount> Limiteds)
        {
            context.LimitedAmount.AddRange(Limiteds);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool BatchDeleteOrganization(long[] Ids)
        {
            IQueryable<Organization> Org = context.Organization.FindBy((Organization item) => Ids.Contains(item.Id));
            context.Organization.RemoveRange(Org);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public List<LimitedAmount> GetLimitedAmount(long Id)
        {
            List<LimitedAmount> limitedLists = new List<LimitedAmount>();
            Organization Org = GetOrganizationById(Id);
            if (Org == null)
                return null;
            IQueryable<LimitedAmount> Limited = from item in context.LimitedAmount
                                     where item.RoleId == Org.UserId
                                     select item;
            foreach(LimitedAmount list in Limited.ToList())
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.RoleId);
                ChemCloud_Dictionaries dictInfo = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DValue == list.CoinType.ToString()&&m.DictionaryTypeId == 1);
                list.CoinTypeName = (dictInfo == null ? "" : dictInfo.DKey);
                limitedLists.Add(list);
            }
            return limitedLists;
        }
        public List<LimitedAmount> GetLimitedAmountByRoleId(long roleId)
        {
            List<LimitedAmount> limitedLists = new List<LimitedAmount>();
            
            IQueryable<LimitedAmount> Limited = from item in context.LimitedAmount
                                                where item.RoleId == roleId
                                                select item;
            foreach (LimitedAmount list in Limited.ToList())
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == list.RoleId);
                ChemCloud_Dictionaries dictInfo = context.ChemCloud_Dictionaries.FirstOrDefault((ChemCloud_Dictionaries m) => m.DValue == list.CoinType.ToString() && m.DictionaryTypeId == 1);
                list.CoinTypeName = (dictInfo == null ? "" : dictInfo.DKey);
                limitedLists.Add(list);
            }
            return limitedLists;
        }
        public LimitedAmount GetLimitedAmountByUserId(long UserId,int CoinType)
        {
            LimitedAmount limited = context.LimitedAmount.FirstOrDefault((LimitedAmount m) => m.RoleId == UserId && m.CoinType == CoinType);
            return limited;
        }
        public IQueryable<QueryMember> GetAllPermissionGroup(long userId)
        {
            IQueryable<QueryMember> UserInfo = from item in context.PurchaseRolesInfo
                                               where item.MasterId == userId
                                               select new QueryMember { Id = item.Id, Username = item.RoleName }; 

            return UserInfo;
        }
        public Organization GetOrganizationByRoleId(long roleId)
        {
            Organization Org = (from item in context.Organization
                                where item.RoleId == roleId
                                select item).FirstOrDefault();
            if (Org != null)
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Org.UserId);
                UserMemberInfo UserInfos = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Org.ParentId);
                PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Org.RoleId);
                PurchaseRolesInfo roleInfos = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Org.ParentRoleId);
                Org.UserName = (userInfo == null ? "" : userInfo.UserName);
                Org.RealName = (userInfo == null ? "" : userInfo.RealName);
                Org.ParentName = (UserInfos == null ? "" : UserInfos.UserName);
                Org.ParentRoleName = (roleInfos == null ? "" : roleInfos.RoleName);
                Org.RoleName = roleInfo.RoleName;
            }
            return Org;
        }
        public Organization GetOrganizationById(long Id)
        {
            Organization Org = (from item in context.Organization
                                where item.Id == Id
                                select item).FirstOrDefault(); 
            if (Org != null)
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Org.UserId);
                UserMemberInfo UserInfos = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Org.ParentId);
                PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Org.RoleId);
                PurchaseRolesInfo roleInfos = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Org.ParentRoleId);
                Org.UserName = (userInfo == null ? "" : userInfo.UserName);
                Org.RealName = (userInfo == null ? "" : userInfo.RealName);
                Org.ParentName = (UserInfos == null ? "" : UserInfos.UserName);
                Org.ParentRoleName = (roleInfos == null ? "" : roleInfos.RoleName);
                Org.RoleName = roleInfo.RoleName;
            }
            return Org;
        }
        public Organization GetOrganizationByUserId(long UserId)
        {
            Organization Org = (from item in context.Organization
                                where item.UserId == UserId
                                select item).FirstOrDefault();
            if (Org != null)
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Org.UserId);
                UserMemberInfo UserInfos = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id == Org.ParentId);
                PurchaseRolesInfo roleInfo = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Org.RoleId);
                PurchaseRolesInfo roleInfos = context.PurchaseRolesInfo.FirstOrDefault((PurchaseRolesInfo m) => m.Id == Org.ParentRoleId);
                Org.UserName = (userInfo == null ? "" : userInfo.UserName);
                Org.RealName = (userInfo == null ? "" : userInfo.RealName);
                Org.ParentName = (UserInfos == null ? "" : UserInfos.UserName);
                Org.ParentRoleName = (roleInfos == null ? "" : roleInfos.RoleName);
                Org.RoleName = roleInfo.RoleName;
            }
            return Org;
        }
        public bool IsExitsOrganization(long roleId)
        {
            Organization Orgs = context.Organization.FirstOrDefault((Organization m) => m.ParentRoleId == roleId);
            if (Orgs != null)
                return true;
            else
                return false;
        }
        public bool IsExitsOrganizationByUserId(long userId)
        {
            Organization Orgs = context.Organization.FirstOrDefault((Organization m) => m.UserId == userId);
            if (Orgs != null)
                return true;
            else
                return false;
        }
        public bool DeleteOrgannization(long Id)
        {
            int i = 0;
            Organization Org = context.Organization.FirstOrDefault((Organization m) => m.Id == Id);
            if (Org != null)
            {
                context.Organization.Remove(Org);
                i = context.SaveChanges();
            }
            if (i > 0)
                return true;
            else
                return false;
        }


        public bool AmountOver(long id, decimal money, int CoinType)
        {
           Organization Org = GetOrganizationByUserId(id);
           LimitedAmount la = (from a in context.LimitedAmount where a.RoleId==Org.RoleId && a.CoinType==CoinType select a ).FirstOrDefault<LimitedAmount>();
           if (la == null || la.Id == 0)
           {
               return false;
           }
           else
           {
               return la.Money > money;
           }
        }
        public LimitedAmount GetlimitedByRoleId(long RoleId, int CoinType)
        {
            LimitedAmount limited = context.LimitedAmount.FirstOrDefault((LimitedAmount m)=>m.RoleId == RoleId && m.CoinType == CoinType);
            return limited;
        }
        public List<Organization> GetAllPerson(long roleId)
        {
            Organization Org = new Organization();
            List<Organization> UserInfo = new List<Organization>();
            IQueryable<Organization> orgInfo = from item in context.Organization
                                               where item.RoleId == roleId
                                               select item;
            foreach (Organization org in orgInfo.ToList())
            {
                UserInfo.Add(GetOrganizationById(org.Id));
            }
            return UserInfo;
        }
    }
}
