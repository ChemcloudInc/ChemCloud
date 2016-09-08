using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 收入
    /// </summary>
    public class Finance_InCome : BaseModel
    {
        public Finance_InCome() { }
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
        /// 收入单号
        /// </summary>
        public long InCome_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 收入人编号
        /// </summary>
        public long InCome_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 收入人类型
        /// </summary>
        public int InCome_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 1收入时间2收入周期开始时间
        /// </summary>
        public DateTime InCome_StartTime
        {
            set;
            get;
        }
        /// <summary>
        /// 收入周期结束时间
        /// </summary>
        public DateTime? InCome_EndTime
        {
            set;
            get;
        }
        /// <summary>
        /// 收入的金额
        /// </summary>
        public decimal InCome_Money
        {
            set;
            get;
        }
        /// <summary>
        /// 收入的币种
        /// </summary>
        public int InCome_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public long InCome_OrderNum
        {
            set;
            get;
        }
        /// <summary>
        /// IP
        /// </summary>
        public string InCome_Address
        {
            set;
            get;
        }
        /// <summary>
        /// 收入类型(1交易2转账3退款默认1)
        /// </summary>
        public int InCome_Type
        {
            set;
            get;
        }
        /// <summary>
        /// 状态(0锁定1正常默认1)
        /// </summary>
        public int InCome_Status
        {
            set;
            get;
        }
    }
}
