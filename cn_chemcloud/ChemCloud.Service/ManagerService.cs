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
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace ChemCloud.Service
{
    public class ManagerService : ServiceBase, IManagerService, IService, IDisposable
    {
        public ManagerService()
        {
        }

        public void AddPlatformManager(ManagerInfo model)
        {
            if (model.RoleId == 0)
            {
                throw new HimallException("权限组选择不正确!");
            }
            if (CheckUserNameExist(model.UserName, true))
            {
                throw new HimallException("该用户名已存在！");
            }
            model.ShopId = 0;
            model.PasswordSalt = Guid.NewGuid().ToString();
            model.CreateDate = DateTime.Now;
            string str = SecureHelper.MD5(model.Password);
            model.Password = SecureHelper.MD5(string.Concat(str, model.PasswordSalt));
            context.ManagerInfo.Add(model);
            context.SaveChanges();
        }

        public bool AddSellerManager(ManagerInfo model, string currentSellerName)
        {
            if (model.RoleId == 0)
            {
                throw new HimallException("权限组选择不正确!");
            }
            if (CheckUserNameExist(model.UserName, false))
            {
                throw new HimallException("该用户名已存在！");
            }
            if (model.ShopId == 0)
            {
                throw new HimallException("没有权限进行该操作！");
            }
            model.PasswordSalt = Guid.NewGuid().ToString();
            model.CreateDate = DateTime.Now;
            string str = SecureHelper.MD5(model.Password);
            model.Password = SecureHelper.MD5(string.Concat(str, model.PasswordSalt));
            context.ManagerInfo.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool UpdateSellerManager(string username, long shopId, long roleId)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo p) => p.UserName == username && p.ShopId == shopId);
            managerInfo.RoleId = roleId;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 在新增HiMall_Manager的时候给Shops表中加了一条数据，后面关于这条数据就都是修改（方法：CreateEmptyShop（））
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public ManagerInfo AddSellerManager(string username, string password, string salt = "")
        {
            ManagerInfo managerInfo;
            ManagerInfo managerInfo1 = context.ManagerInfo.FirstOrDefault((ManagerInfo p) => p.UserName == username);
            if (managerInfo1 != null)
            {
                return new ManagerInfo()
                {
                    Id = managerInfo1.Id
                };
            }
            if (string.IsNullOrEmpty(salt))
            {
                Guid guid = Guid.NewGuid();
                salt = SecureHelper.MD5(guid.ToString("N"));
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                ShopInfo shopInfo = Instance<IShopService>.Create.CreateEmptyShop();
                FieldCertification fieldfertification = Instance<ICertification>.Create.CreateFieldModulep();
                ManagerInfo managerInfo2 = new ManagerInfo()
                {
                    CreateDate = DateTime.Now,
                    UserName = username,
                    Password = password,
                    PasswordSalt = salt,
                    ShopId = shopInfo.Id,

                    CertificationId = fieldfertification.Id,
                    SellerPrivileges = new List<SellerPrivilege>()
					{
						0
					},
                    AdminPrivileges = new List<AdminPrivilege>(),
                    RoleId = 0
                };

                managerInfo = managerInfo2;
                context.ManagerInfo.Add(managerInfo);
                context.SaveChanges();
                transactionScope.Complete();
            }
            return managerInfo;
        }

        public void BatchDeletePlatformManager(long[] ids)
        {
            IQueryable<ManagerInfo> managerInfos = context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == 0 && item.RoleId != 0 && ids.Contains(item.Id));
            context.ManagerInfo.RemoveRange(managerInfos);
            context.SaveChanges();
        }

        public void BatchDeleteSellerManager(long[] ids, long shopId)
        {
            IQueryable<ManagerInfo> managerInfos = context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == shopId && item.RoleId != 0 && ids.Contains(item.Id));
            context.ManagerInfo.RemoveRange(managerInfos);
            context.SaveChanges();
        }

        public void ChangePlatformManagerPassword(long id, string password)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0).FirstOrDefault();
            if (managerInfo == null)
            {
                throw new HimallException("该管理员不存在，或者已被删除!");
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                string str = SecureHelper.MD5(password);
                managerInfo.Password = SecureHelper.MD5(string.Concat(str, managerInfo.PasswordSalt));
            }
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Manager(id));
        }
        public void ChangePlatformManager(long id, long roleId)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0).FirstOrDefault();
            if (managerInfo == null)
            {
                throw new HimallException("该管理员不存在，或者已被删除!");
            }
            if (roleId != 0 && managerInfo.RoleId != 0)
            {
                managerInfo.RoleId = roleId;
            }
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Manager(id));
        }
        public bool ChangeSellerManager(ManagerInfo info, UserMemberInfo userInfo)
        {
            ManagerInfo roleId = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == info.Id && item.ShopId == info.ShopId).FirstOrDefault();
            UserMemberInfo memberInfo = context.UserMemberInfo.FindById<UserMemberInfo>(userInfo.Id);
            if (roleId == null || memberInfo == null)
            {
                throw new HimallException("该管理员不存在，或者已被删除!");
            }
            if (info.RoleId != 0 && roleId.RoleId != 0)
            {
                roleId.RoleId = info.RoleId;
            }
            if (!string.IsNullOrWhiteSpace(info.Password))
            {
                string str = SecureHelper.MD5(info.Password);
                roleId.Password = SecureHelper.MD5(string.Concat(str, roleId.PasswordSalt));
                memberInfo.Password = SecureHelper.MD5(string.Concat(userInfo.Password, memberInfo.PasswordSalt));
            }
            roleId.RealName = info.RealName;
            roleId.Remark = info.Remark;
            memberInfo.RealName = userInfo.RealName;
            memberInfo.Remark = userInfo.Remark;
            if (userInfo.Email != memberInfo.Email)
            {
                memberInfo.Email = userInfo.Email;
            }
            int i = context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Seller(info.Id));
            Cache.Remove(CacheKeyCollection.Member(userInfo.Id));
            if (i >= 0)
                return true;
            else
                return false;
        }

        public void ChangeSellerManagerPassword(long id, long shopId, string password, long roleId)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == shopId).FirstOrDefault();
            if (managerInfo == null)
            {
                throw new HimallException("该管理员不存在，或者已被删除!");
            }
            if (roleId != 0 && managerInfo.RoleId != 0)
            {
                managerInfo.RoleId = roleId;
            }
            if (!string.IsNullOrWhiteSpace(password))
            {
                string str = SecureHelper.MD5(password);
                managerInfo.Password = SecureHelper.MD5(string.Concat(str, managerInfo.PasswordSalt));
            }
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Seller(id));
        }

        public bool CheckUserNameExist(string username, bool isPlatFormManager = false)
        {
            bool flag = false;
            if (!isPlatFormManager)
            {
                UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.UserName.ToLower() == username.ToLower());
                if (userInfo != null)
                {
                    flag = true;
                }
            }
            else
            {
                ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo item) => (item.UserName.ToLower() == username.ToLower()) && item.ShopId == 0);
                if (managerInfo != null)
                {
                    flag = true;
                }
            }
            if (flag)
                return true;
            else
                return false;
        }

        public void DeletePlatformManager(long id)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == 0 && item.RoleId != 0).FirstOrDefault();
            context.ManagerInfo.Remove(managerInfo);
            context.SaveChanges();
        }

        public void DeleteSellerManager(long id, long shopId)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FindBy((ManagerInfo item) => item.Id == id && item.ShopId == shopId && item.RoleId != 0).FirstOrDefault();
            context.ManagerInfo.Remove(managerInfo);
            context.SaveChanges();
        }

        public IQueryable<ManagerInfo> GetManagers(string keyWords)
        {
            return context.ManagerInfo.FindBy((ManagerInfo item) => keyWords == null || (keyWords == "") || item.UserName.Contains(keyWords));
        }

        private string GetPasswrodWithTwiceEncode(string password, string salt)
        {
            string str = SecureHelper.MD5(password);
            return SecureHelper.MD5(string.Concat(str, salt));
        }

        public ManagerInfo GetPlatformManager(long userId)
        {
            ManagerInfo roleName = null;
            string str = CacheKeyCollection.Manager(userId);
            if (Cache.Get(str) == null)
            {
                roleName = context.ManagerInfo.FirstOrDefault((ManagerInfo item) => item.Id == userId && item.ShopId == 0);
                if (roleName == null)
                {
                    return null;
                }
                if (roleName.RoleId != 0)
                {
                    RoleInfo roleInfo = context.RoleInfo.FindById<RoleInfo>(roleName.RoleId);
                    if (roleInfo != null)
                    {
                        List<AdminPrivilege> adminPrivileges = new List<AdminPrivilege>();
                        roleInfo.RolePrivilegeInfo.ToList().ForEach((RolePrivilegeInfo a) => adminPrivileges.Add((AdminPrivilege)a.Privilege));
                        roleName.RoleName = roleInfo.RoleName;
                        roleName.AdminPrivileges = adminPrivileges;
                        roleName.Description = roleInfo.Description;
                    }
                }
                else
                {
                    List<AdminPrivilege> adminPrivileges1 = new List<AdminPrivilege>()
					{
						0
					};
                    roleName.RoleName = "系统管理员";
                    roleName.AdminPrivileges = adminPrivileges1;
                    roleName.Description = "系统管理员";
                }
                Cache.Insert(str, roleName);
            }
            else
            {
                roleName = (ManagerInfo)Cache.Get(str);
            }
            return roleName;
        }

        public IQueryable<ManagerInfo> GetPlatformManagerByRoleId(long roleId)
        {
            return context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == 0 && item.RoleId == roleId);
        }

        public PageModel<ManagerInfo> GetPlatformManagers(ManagerQuery query)
        {
            int num = 0;
            IQueryable<ManagerInfo> managerInfos = context.ManagerInfo.FindBy<ManagerInfo, long>((ManagerInfo item) => item.ShopId == 0, query.PageNo, query.PageSize, out num, (ManagerInfo item) => item.RoleId, true);
            return new PageModel<ManagerInfo>()
            {
                Models = managerInfos,
                Total = num
            };
        }

        public ManagerInfo GetSellerManager(long userId)
        {
            ManagerInfo roleName = null;
            string str = CacheKeyCollection.Seller(userId);
            if (Cache.Get(str) == null)
            {
                roleName = (
                    from item in context.ManagerInfo
                    where item.Id == userId && item.ShopId != 0
                    select item).FirstOrDefault();
                if (roleName == null)
                {
                    return null;
                }
                if (roleName.RoleId != 0)
                {
                    RoleInfo roleInfo = context.RoleInfo.FindById<RoleInfo>(roleName.RoleId);
                    if (roleInfo != null)
                    {
                        List<SellerPrivilege> sellerPrivileges = new List<SellerPrivilege>();
                        roleInfo.RolePrivilegeInfo.ToList().ForEach((RolePrivilegeInfo a) => sellerPrivileges.Add((SellerPrivilege)a.Privilege));
                        roleName.RoleName = roleInfo.RoleName;
                        roleName.SellerPrivileges = sellerPrivileges;
                        roleName.Description = roleInfo.Description;


                    }
                }
                else
                {
                    List<SellerPrivilege> sellerPrivileges1 = new List<SellerPrivilege>()
					{
						0
					};
                    roleName.RoleName = "系统管理员";
                    roleName.SellerPrivileges = sellerPrivileges1;
                    roleName.Description = "系统管理员";
                }
                Cache.Insert(str, roleName);
            }
            else
            {
                roleName = (ManagerInfo)Cache.Get(str);
            }
            if (roleName != null)
            {
                VShopInfo vShopInfo = context.VShopInfo.FirstOrDefault((VShopInfo item) => item.ShopId == roleName.ShopId);
                roleName.VShopId = -1;
                if (vShopInfo != null)
                {
                    roleName.VShopId = vShopInfo.Id;
                }
            }
            return roleName;
        }

        public ManagerInfo GetSellerManager(string userName)
        {
            ManagerInfo managerInfo = (
                from item in context.ManagerInfo
                where (item.UserName == userName) && item.ShopId != 0
                select item).FirstOrDefault();
            return managerInfo;
        }

        public IQueryable<ManagerInfo> GetSellerManagerByRoleId(long roleId, long shopId)
        {
            return context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == shopId && item.RoleId == roleId);
        }

        public PageModel<ManagerInfo> GetSellerManagers(ManagerQuery query)
        {
            int num = 0;
            IQueryable<ManagerInfo> managerInfos = from item in base.context.ManagerInfo
                                                   where item.ShopId == query.ShopID
                                                   select item;
            if (query.roleID != 0)
            {
                managerInfos = context.ManagerInfo.FindBy((ManagerInfo item) => item.ShopId == query.ShopID && item.RoleId == query.roleID && item.Id == query.userID);
            }

            Func<IQueryable<ManagerInfo>, IOrderedQueryable<ManagerInfo>> func = null;
            func = (IQueryable<ManagerInfo> d) =>
                    from o in d
                    orderby o.CreateDate descending
                    select o;
            num = managerInfos.Count();
            managerInfos = managerInfos.GetPage(out num, query.PageNo, query.PageSize);
            foreach (ManagerInfo list in managerInfos.ToList())
            {
                ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.Id.Equals(list.Id));
                if (managerInfo != null)
                {
                    UserMemberInfo userInfo = context.UserMemberInfo.First((UserMemberInfo m) => m.UserName.Equals(managerInfo.UserName));
                    list.Email = (userInfo == null ? "" : userInfo.Email);
                    if (managerInfo.RoleId == 0)
                        managerInfo.RoleName = "管理员";
                    else
                    {
                        RoleInfo role = context.RoleInfo.FirstOrDefault((RoleInfo m) => m.ShopId == managerInfo.ShopId && m.Id == managerInfo.RoleId);
                        managerInfo.RoleName = role.RoleName;
                    }
                }
            }
            return new PageModel<ManagerInfo>()
            {
                Models = managerInfos,
                Total = num
            };
        }

        public ManagerInfo Login(string username, string password, bool isPlatFormManager = false)
        {
            ManagerInfo managerInfo;
            managerInfo = (!isPlatFormManager ? context.ManagerInfo.FindBy((ManagerInfo item) => (item.UserName == username) && item.ShopId != 0).FirstOrDefault() : context.ManagerInfo.FindBy((ManagerInfo item) => (item.UserName == username) && item.ShopId == 0).FirstOrDefault());
            if (managerInfo != null)
            {
                if (GetPasswrodWithTwiceEncode(password, managerInfo.PasswordSalt).ToLower() != managerInfo.Password)
                {
                    managerInfo = null;
                }
                else if (managerInfo.ShopId > 0)
                {
                    ShopInfo shop = Instance<IShopService>.Create.GetShop(managerInfo.ShopId, false);
                    if (shop == null)
                    {
                        throw new HimallException("未找到帐户对应的供应商");
                    }
                    if (!shop.IsSelf && shop.ShopStatus == ShopInfo.ShopAuditStatus.Freeze)
                    {
                        throw new HimallException("帐户所在供应商已被冻结");
                    }
                }
            }
            return managerInfo;
        }

        public void UpdateShopStatus()
        {
            List<ShopInfo> list = (
                from s in context.ShopInfo
                where s.EndDate < DateTime.Now
                select s).ToList();
            foreach (ShopInfo nullable in list)
            {
                if (!nullable.IsSelf)
                {
                    nullable.ShopStatus = ShopInfo.ShopAuditStatus.Unusable;
                }
                else
                {
                    DateTime now = DateTime.Now;
                    nullable.EndDate = new DateTime?(now.AddYears(10));
                    nullable.ShopStatus = ShopInfo.ShopAuditStatus.Open;
                }
            }
            context.SaveChanges();
        }

        public UserMemberInfo GetMemberIdByShopId(long shopId)
        {
            ManagerInfo managerInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo m) => m.ShopId.Equals(shopId));
            string username = (managerInfo == null ? "" : managerInfo.UserName);

            UserMemberInfo _UserMemberInfo = new UserMemberInfo();
            _UserMemberInfo = context.UserMemberInfo.Where(q => q.UserName.Equals(username)).FirstOrDefault();

            return _UserMemberInfo;
        }

        public ManagerInfo GetManageInfoByPwd(long manId, string password)
        {
            ManagerInfo userMemberInfo = context.ManagerInfo.FirstOrDefault((ManagerInfo e) => e.Id == manId);
            if (userMemberInfo != null && SecureHelper.MD5(string.Concat(SecureHelper.MD5(password.Trim()), userMemberInfo.PasswordSalt)) == userMemberInfo.Password)
            {
                return userMemberInfo;
            }
            return null;
        }

        public List<UserMemberInfo> GetMenberIdByShopState(int typeid)
        {
            List<ShopInfo> shopList = new List<ShopInfo>();
            if (typeid == 0)
            {
                shopList = (from a in context.ShopInfo select a).ToList();
            }
            else
            {
                shopList = (from a in context.ShopInfo where a.GradeId == typeid select a).ToList();
            }
            List<UserMemberInfo> list = new List<UserMemberInfo>();
            foreach (ShopInfo item in shopList)
            {
                UserMemberInfo u = GetMemberIdByShopId(item.Id);
                if (u != null)
                {
                    list.Add(u);
                }

            }
            //if (list.Count == 0)
            //{
            //    return null;
            //}
            //List<UserMemberInfo> newList = new List<UserMemberInfo>();
            //if (!string.IsNullOrWhiteSpace(Search))
            //{
            //    foreach (UserMemberInfo item in list)
            //    {
            //        if (item.UserName.Contains(Search))
            //        {
            //            newList.Add(item);
            //        }
            //    }
            //}
            return list;
        }
        public List<UserMemberInfo> GetMenberIdByName(string Search)
        {
            List<UserMemberInfo> list = new List<UserMemberInfo>();
            list = (from a in context.UserMemberInfo where a.UserName.Contains(Search) select a).ToList();
            return list;
        }
        public ManagerInfo GetManagerInfoByShopId(long shopId)
        {
            if (shopId != 0)
            {
                ManagerInfo minfo = (from p in context.ManagerInfo where p.ShopId.Equals(shopId) select p).FirstOrDefault();
                return minfo;
            }
            else
            {
                return null;
            }
        }


        public ManagerInfo GetManagerInfoByUserName(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                ManagerInfo _ManagerInfo = (from p in context.ManagerInfo where p.UserName.Equals(username) select p).FirstOrDefault();
                if (_ManagerInfo != null) { return _ManagerInfo; }
                else { return null; }
            }
            else
            {
                return null;
            }
        }
    }
}