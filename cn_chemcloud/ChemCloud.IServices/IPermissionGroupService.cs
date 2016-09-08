using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.IServices
{
    public interface IPermissionGroupService : IService, IDisposable
    {
        PurchaseRolesInfo GetPurchaseRole(long Id);

        List<PurchaseRolesInfo> GetPurchaseRolesByParentId(long ParentId);


        PageModel<PurchaseRolesInfo> GetPurchaseRoles(long Id, int page, int rows);

        PurchaseRolesInfo AddPermissionGroup(long parentId, string rolename);

        bool UpdatePermissionGroup(long Id, string RoleName);

        bool Delete(long Id, long[] Ids);

        bool BatchDelete(long[] Ids, long[] RoleIds);

        PurchaseRolesInfo GetPurchaseRoleByMasterId(long MasterId, string RoleName);

        PurchaseRolesInfo GetPurchaseRoleByUserId(long MasterId);
    }
}
