using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;
using ChemCloud.QueryModel;
using ChemCloud.Model.Common;
namespace ChemCloud.IServices
{
    public interface ILawInfoService : IService, IDisposable
    {
        PageModel<LawInfo> GetLawInfos(LawInfoQuery model, long? userId);

        LawInfo GetLawInfo(long Id);

        AttachmentInfo GetAttachmentInfo(long Id);

        LawInfo  AddLawInfo(LawInfo model);

        bool AddAttachment(AttachmentInfo model);

        bool UpdateLawInfo(LawInfo model);

        bool UpdateAttachment(AttachmentInfo model);

        bool DeleteLawInfo(long Id);

        AttachmentInfo GetAttachmentInfoById(long Id);

        bool BatchDelete(long[] Ids);

        bool UpdateLawInfoStatus(long Id, int status, long userId);

        List<AttachmentInfo> GetAttachmentInfosById(long Id);

        bool DeleteAttachment(long Id);

        #region 前台 会议中心
        Result_LawInfo GetObjectById_Web(int Id);

        Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id);
        Result_List<Result_Model<LawInfo>> Get_PreNext_ById_Web(long Id);
        Result_Model<PageInfo> Get_PageInfo_Web(QueryCommon<LawInfoQuery_Web> query);
        Result_List_Pager<Result_LawInfo> GetLawInfoList_Web(QueryCommon<LawInfoQuery_Web> query);
        #endregion
    }
}
