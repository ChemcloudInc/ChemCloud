using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemCloud.IServices
{
    public interface IManagerService : IService, IDisposable
    {
        void AddPlatformManager(ManagerInfo model);

        ManagerInfo AddSellerManager(string username, string password, string salt);

        bool AddSellerManager(ManagerInfo model, string currentSellerName);

        void BatchDeletePlatformManager(long[] ids);

        void BatchDeleteSellerManager(long[] ids, long shopId);
        void ChangePlatformManager(long id, long roleId);
        void ChangePlatformManagerPassword(long id, string password);

        bool ChangeSellerManager(ManagerInfo info, UserMemberInfo userInfo);

        void ChangeSellerManagerPassword(long id, long shopId, string password, long roleId);

        bool CheckUserNameExist(string userName, bool isPlatFormManager = false);

        void DeletePlatformManager(long id);

        void DeleteSellerManager(long id, long shopId);

        IQueryable<ManagerInfo> GetManagers(string keyWords);

        ManagerInfo GetPlatformManager(long userId);

        IQueryable<ManagerInfo> GetPlatformManagerByRoleId(long roleId);

        PageModel<ManagerInfo> GetPlatformManagers(ManagerQuery query);

        ManagerInfo GetSellerManager(long userId);

        ManagerInfo GetSellerManager(string userName);

        IQueryable<ManagerInfo> GetSellerManagerByRoleId(long roleId, long shopId);

        PageModel<ManagerInfo> GetSellerManagers(ManagerQuery query);

        ManagerInfo Login(string username, string password, bool isPlatFormManager = false);

        void UpdateShopStatus();
        bool UpdateSellerManager(string username, long shopId, long roleId);
        UserMemberInfo GetMemberIdByShopId(long shopId); //根据shopid获取到对应在chemcloud表的id

        ManagerInfo GetManageInfoByPwd(long manId, string password);

        List<UserMemberInfo> GetMenberIdByShopState(int typeid);

        List<UserMemberInfo> GetMenberIdByName(string Search);
        /// <summary>
        /// 根据店铺编号查询店铺的用户信息 一条记录  xue
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        ManagerInfo GetManagerInfoByShopId(long shopId);

        ManagerInfo GetManagerInfoByUserName(string username);
    }
}