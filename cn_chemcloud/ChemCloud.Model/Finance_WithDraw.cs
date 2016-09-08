using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 提现
    /// </summary>
    public class Finance_WithDraw : BaseModel
    {
        public Finance_WithDraw() { }
        private long _id;
        /// <summary>
        /// 编号(自动标识)
        /// </summary>
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
        /// 提现单号
        /// </summary>
        public long Withdraw_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 提现用户编号
        /// </summary>
        public long Withdraw_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 提现用户类型
        /// </summary>
        public int Withdraw_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Withdraw_Money
        {
            set;
            get;
        }
        /// <summary>
        /// 提现币种
        /// </summary>
        public int Withdraw_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 提现银行名称
        /// </summary>
        public string Withdraw_BankName { get; set; }
        /// <summary>
        /// 提现法人
        /// </summary>
        public string Withdraw_BankUserName { get; set; }
        /// <summary>
        /// 提现卡号
        /// </summary>
        public string Withdraw_Account
        {
            set;
            get;
        }
        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime Withdraw_Time
        {
            set;
            get;
        }
        /// <summary>
        /// 提现状态(0未成功提现1已成功提现)
        /// </summary>
        public int Withdraw_Status
        {
            set;
            get;
        }
        /// <summary>
        /// 提现审核状态(0待审核1审核通过2审核拒绝)
        /// </summary>
        public int Withdraw_shenhe
        {
            set;
            get;
        }
        /// <summary>
        /// 审核人编号
        /// </summary>
        public long Withdraw_shenheUid
        {
            set;
            get;
        }
        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string Withdraw_shenheUname
        {
            set;
            get;
        }
        /// <summary>
        /// 审核结果理由
        /// </summary>
        public string Withdraw_shenheDesc
        {
            set;
            get;
        }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime Withdraw_shenheTime
        {
            set;
            get;
        }

        /// <summary>
        /// 提现用户名称
        /// </summary>
        [NotMapped]
        public string Withdraw_UserName { get; set; }
    }
}
