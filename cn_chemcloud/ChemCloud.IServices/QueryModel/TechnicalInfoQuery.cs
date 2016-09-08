using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class TechnicalInfoQuery : QueryBase
    {
        public string Title { get; set; }
        public int? Status { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public int LanguageType { get; set; }
    }
    public class TechnicalInfoAddQuery : QueryBase
    {
        public string Title { get; set; }

        public string TechContent { get; set; }
        public int Status { get; set; }

        public string Author { get; set; }

        public string Tel { get; set; }

        public string Email { get; set; }

        public int LanguageType { get; set; }

    }
    #region 前台  会议中心
    public class TechnicalInfoQuery_Web : QueryBase
    {
        public string Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 会议类型 0：所有；1：最新；2：历史
        /// </summary>
        public int Type { get; set; }

        public int LanguageType { get; set; }

    }

    #endregion
    public class TechnicalInfoModifyQuery : TechnicalInfoAddQuery
    {
        public long Id { get; set; }

    }
}
