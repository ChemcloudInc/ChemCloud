using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;

namespace ChemCloud.IServices
{
    public interface IOrganizationService : IService, IDisposable
    {
        /// <summary>
        /// 获取组织架构
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<Organization> GetOrganizations(long UserId);
        /// <summary>
        /// 添加组织架构
        /// </summary>
        /// <param name="orga"></param>
        /// <returns></returns>
        bool AddOrganization(Organization orga);
        /// <summary>
        /// 编辑组织架构
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        bool UpdateOrganization(Organization orga);
        /// <summary>
        /// 删除组织架构
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        bool DeleteOrganization(long Id);
        /// <summary>
        /// 批量删除组织架构
        /// </summary>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        bool BatchDeleteOrganization(long[] Ids);
        /// <summary>
        /// 获取限制金额
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<LimitedAmount> GetLimitedAmount(long Id);
        
        /// <summary>
        /// 获取所有员工
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IQueryable<QueryMember> GetAllPermissionGroup(long userId);
        /// <summary>
        /// 根据Id获取组织架构
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Organization GetOrganizationById(long Id);
        /// <summary>
        /// 验证该组织架构下是否有下级架构
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool IsExitsOrganization(long RoleId);
        /// <summary>
        /// 添加金额限制
        /// </summary>
        /// <param name="Limiteds"></param>
        /// <returns></returns>
        bool AddLimited(List<LimitedAmount> Limiteds);
        /// <summary>
        /// 根据UserId获取组织架构
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Organization GetOrganizationByUserId(long UserId);
        /// <summary>
        /// 编辑限制金额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateLimitedAmount(LimitedAmount model);
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        List<Organization> ChildOrganization(long Id);

        //LimitedAmount GetLimitedAmountByUserId(long UserId, int CoinType);

        bool AmountOver(long id, decimal money, int CoinType);

        LimitedAmount GetlimitedByRoleId(long RoleId, int CoinType);

        List<Organization> GetAllPerson(long MasterId);

        List<LimitedAmount> GetLimitedAmountByRoleId(long roleId);

        Organization GetOrganizationByRoleId(long roleId);

        bool IsExitsOrganizationByUserId(long userId);

        bool IsAdmin(long Id);
    }
}
