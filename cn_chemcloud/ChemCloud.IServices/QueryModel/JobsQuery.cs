using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class JobsQuery
    {
        public string JobTitle { get; set; }
        public long? UserId { get; set; }
        public int? UserType { get; set; }


        public decimal? PayrolHigh { get; set; }
        public decimal? PayrolLow { get; set; }

        /// <summary>
        /// 薪资类型（0：面议；1：时薪；2：月薪；3：年薪）
        /// </summary>
        public int PayrollType { get; set; }
        /// <summary>
        /// 工作类型（0：全职；1：兼职）
        /// </summary>
        public int WorkType { get; set; }
        /// <summary>
        /// 审核状态（1：待审核；2：审核未通过；3：审核已通过）
        /// </summary>
        public int ApprovalStatus { get; set; }

        public int LanguageType { get; set; }

    }

    public class JobsAddQuery : QueryBase
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobContent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UserId { get; set; }
        public int UserType { get; set; }
        public int ApprovalStatus { get; set; }
        public int Reviewer { get; set; }

        public decimal PayrolHigh { get; set; }
        public decimal PayrolLow { get; set; }
        /// <summary>
        /// 货币类型（货币类型（0：美元$；1：人民币：￥））
        /// </summary>
        public int TypeOfCurrency { get; set; }
        /// <summary>
        /// 薪资类型（0：面议；1：时薪；2：月薪；3：年薪）
        /// </summary>
        public int PayrollType { get; set; }
        /// <summary>
        /// 工作类型（0：全职；1：兼职）
        /// </summary>
        public int WorkType { get; set; }
        public string WorkPlace { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyEmail { get; set; }

        public int LanguageType { get; set; }
    }

    public class JobsModifyQuery : JobsAddQuery
    {
        public long Id { get; set; }

    }
}
