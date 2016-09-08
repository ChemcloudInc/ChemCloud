using ChemCloud.IServices.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.IServices.QueryModel
{
    public class LawInfoQuery : QueryBase
    {
        public string Title { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public int LanguageType { get; set; }
    }
    public class LawInfoAddQuery : QueryBase
    {
        public string Title { get; set; }

        public string LawsInfo { get; set; }
        public int Status { get; set; }      

        public string Author { get; set; }

        public int LanguageType { get; set; }

    }

    public class LawInfoModifyQuery : LawInfoAddQuery
    {
        public long Id { get; set; }

    }

    #region 前台  会议中心
    public class LawInfoQuery_Web : QueryBase
    {
        public string Title { get; set; }
        public string LawsInfo { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int UserId { get; set; }
        public int LanguageType { get; set; }

    }
    #endregion
}
