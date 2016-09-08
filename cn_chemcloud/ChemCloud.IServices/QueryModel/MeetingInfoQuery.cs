using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.QueryModel
{
    public class MeetingInfoQuery : QueryBase
    {
        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int LanguageType { get; set; }
    }

    #region 前台  会议中心
    public class MeetingInfoQuery_Web
    {
        /// <summary>
        /// 会议类型 0：所有；1：最新；2：历史
        /// </summary>
        public int Type { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int UserId { get; set; }

        public int LanguageType { get; set; }

    }

    #endregion
    public class MeetingInfoAddQuery : QueryBase
    {
        public string Title { get; set; }
        public DateTime MeetingTime { get; set; }
        public string MeetingPlace { get; set; }
        public string MeetingContent { get; set; }

        public string ContinueTime { get; set; }

        public int LanguageType { get; set; }
    }

    public class MeetingInfoModifyQuery : MeetingInfoAddQuery
    {
        public long  Id { get; set; }

    }
}
