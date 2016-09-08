using ChemCloud.Model;
using System;
using System.Linq;

namespace ChemCloud.IServices
{
	public interface IPrivilegesService : IService, IDisposable
	{
        bool AddPlatformRole(RoleInfo model);

		void AddSellerRole(RoleInfo model);

        bool DeletePlatformRoles(long id, long roleId);

        bool DeleteSellerRoles(long shopId, long memberId, long userId);

        bool DeletePlatformRole(long id);

        void DeleteSellerRole(long id, long shopId);

		RoleInfo GetPlatformRole(long id);

		IQueryable<RoleInfo> GetPlatformRoles();

		RoleInfo GetSellerRole(long id, long shopId);

		IQueryable<RoleInfo> GetSellerRoles(long shopId);

		void UpdatePlatformRole(RoleInfo model);

		void UpdateSellerRole(RoleInfo model);

        bool CheckRoleNameExist(string username, long shopId);

        long GetRoleId(string RoleName, long ShopId);

        bool AddChileRole(ManagerInfo managerInfo, UserMemberInfo userInfo);

        long GetPlatformRoleId(string roleName);

        bool BatchDeleteSellerRole(long[] ids, long[] memberIds,long shopId);

        bool BatchDeletPlatformRole(long[] ids, long[] roleIds);

        bool HasMessage(long roleId);
	}
}