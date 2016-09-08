using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;
using ChemCloud.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface ITechnicalInfoService : IService, IDisposable
    {

        PageModel<TechnicalInfo> GetTechInfos(TechnicalInfoQuery model, long? userId);

        TechnicalInfo GetTechInfo(long Id);

        AttachmentInfo GetAttachmentInfo(long Id);

        TechnicalInfo AddTechnicalInfo(TechnicalInfo techInfo);

        bool AddAttachment(AttachmentInfo model);

        bool UpdateTechInfo(TechnicalInfo model);

        Result_Msg UpdateAttachment(AttachmentInfo model);

        bool DeleteTechInfo(long Id);

        AttachmentInfo GetAttachmentInfoById(long Id);

        bool BatchDelete(long[] Ids);

        bool UpdateTechinfoStatus(long Id, int status, long userId);

        List<AttachmentInfo> GetAttachmentInfosById(long Id);

        bool DeleteAttachment(long Id);

        #region 前台 技术资料
        Result_TechnicalInfo GetObjectById_Web(int Id);

        Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id);
        Result_List<Result_Model<TechnicalInfo>> Get_PreNext_ById_Web(long Id);
        Result_Model<PageInfo> Get_PageInfo_Web(QueryCommon<TechnicalInfoQuery_Web> query);

        Result_List_Pager<Result_TechnicalInfo> GetMeetingsList_Web(QueryCommon<TechnicalInfoQuery_Web> query);
        #endregion
    }
}
