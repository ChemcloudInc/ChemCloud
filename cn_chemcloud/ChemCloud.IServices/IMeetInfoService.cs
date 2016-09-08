using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ChemCloud.IServices.QueryModel;
using ChemCloud.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface IMeetInfoService : IService, IDisposable
    {
        PageModel<MeetingInfo> GetMeetingInfos(MeetingInfoQuery model);

        MeetingInfo GetMeetInfo(long Id);
        AttachmentInfo GetAttachmentInfo(long Id);

        MeetingInfo AddMeetInfo(MeetingInfo meetInfo);

        bool AddAttachment(AttachmentInfo model);
        bool UpdateMeetInfo(MeetingInfo model);

        bool UpdateAttachment(AttachmentInfo model);
        bool DeleteMeetingInfo(long Id);
        AttachmentInfo GetAttachmentInfoById(long Id);
        bool BatchDelete(long[] Ids);

        PageModel<MeetingInfo> GetMeetingInfosByOneWeek(MeetingInfoQuery model);

        PageModel<MeetingInfo> GetMeetingInfosByThreeWeeks(MeetingInfoQuery model);

        PageModel<MeetingInfo> GetMeetingInfosByThreeMouth(MeetingInfoQuery model);

        List<AttachmentInfo> GetAttachmentInfosById(long Id);

        bool DeleteAttachment(long Id);

        #region 前台 会议中心
        Result_MeetingInfo GetObjectById_Web(int Id);

        Result_List<Result_AttachmentInfo> GetObjectList_ById_Web(long Id);
        Result_List<Result_Model<MeetingInfo>> Get_PreNext_ById_Web(long Id);
        Result_Model<PageInfo> Get_PageInfo_Web(QueryCommon<MeetingInfoQuery_Web> query);

        Result_List_Pager<Result_MeetingInfo> GetMeetingsList_Web(QueryCommon<MeetingInfoQuery_Web> query);
        #endregion

    }
}
