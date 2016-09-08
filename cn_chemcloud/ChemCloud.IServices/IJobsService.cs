using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemCloud.Model;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface IJobsService : IService, IDisposable
    {

        #region Admin 后台
        Result_Msg DeleteById_Admin(int Id);
        Result_Jobs GetObjectById_Admin(int Id);
        Result_Msg ModifyJob_Fast(JobsModifyQuery query);
        Result_List_Pager<Jobs> GetJobsList_Admin(QueryCommon<JobsQuery> query);
        Result_Model<PageInfo> Get_PageInfo_Admin(QueryCommon<JobsQuery> query);

        #endregion

        #region Web 后台
        Result_Msg AddJob(QueryCommon<JobsAddQuery> query);
        Result_Msg DeleteById(int Id, long userId);
        Jobs GetObjectById(int Id, long userId);

        Result_List_Pager<Jobs> GetJobsList_Member(QueryCommon<JobsQuery> query);
        Result_Model<PageInfo> Get_PageInfo_Member(QueryCommon<JobsQuery> query);

        #endregion

        #region Web 前台展示

        Result_Model<Jobs> GetObjectById_Web(int Id);
        Result_List<Result_Model<Jobs>> Get_PreNext_ById_Web(long id, int workType, int languageType);
        Result_List_Pager<Jobs> GetJobsList_Web(QueryCommon<JobsQuery> query);
        Result_Model<PageInfo> Get_PageInfo(QueryCommon<JobsQuery> query);

        #endregion

        #region Web、Admin 公共方法

        string GetCompanyInfo_ByUserIdAndUserType(long userId);

        Result_Msg ModifyJob(QueryCommon<JobsAddQuery> query);
        Result_Msg ModifyJob_Admin(QueryCommon<JobsAddQuery> query);

        #endregion

    }
}
