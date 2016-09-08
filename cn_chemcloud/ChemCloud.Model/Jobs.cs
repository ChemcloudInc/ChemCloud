using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class Jobs : BaseModel
    {
        private long _id;
        public new long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }

        /// <summary>
        /// 职位标题
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 招聘详情
        /// </summary>
        public string JobContent { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 招聘有效开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 招聘有效截止日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 发布者ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 发布者类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 审核状态（1：待审核；2：审核未通过；3：审核已通过）
        /// </summary>
        public int ApprovalStatus { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
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

    public class Result_Jobs : BaseModel
    {
        /// <summary>
        /// 职位标题
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 招聘详情
        /// </summary>
        public string JobContent { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public string UpdateDate { get; set; }

        /// <summary>
        /// 招聘有效开始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 招聘有效截止日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 发布者ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 发布者类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public string ApprovalStatus { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public int Reviewer { get; set; }
        public decimal PayrolLow { get; set; }
        public decimal PayrolHigh { get; set; }
        public string Payrol_LowHigh { get; set; }
        /// <summary>
        /// 货币类型（货币类型（0：美元$；1：人民币：￥））
        /// </summary>
        public string TypeOfCurrency { get; set; }
        /// <summary>
        /// 薪资类型（0：面议；1：时薪；2：月薪；3：年薪）
        /// </summary>
        public string PayrollType { get; set; }
        /// <summary>
        /// 工作类型（0：全职；1：兼职）
        /// </summary>
        public string WorkType { get; set; }
        public string WorkPlace { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyEmail { get; set; }
        public string LanguageType { get; set; }


    }


    #region 公共：返回 对象 + 提示信息 Result_Model
    /// <summary>
    /// 公共：返回 对象 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_Model_List_Jobs<T> where T : class, new()
    {
        public Result_List<Result_Model<T>> List { get; set; }
        public Result_Model<T> Model { get; set; }
        public Result_Msg Msg { get; set; }

    }
    #endregion


}
