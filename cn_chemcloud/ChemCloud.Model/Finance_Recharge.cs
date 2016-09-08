using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 充值
    /// </summary>
    public class Finance_Recharge : BaseModel
    {
        public Finance_Recharge() { }
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
        /// 充值单号
        /// </summary>
        public long Recharge_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 充值用户编号
        /// </summary>
        public long Recharge_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 充值用户类型
        /// </summary>
        public int Recharge_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime Recharge_Time
        {
            set;
            get;
        }
        /// <summary>
        /// 充值IP地址
        /// </summary>
        public string Recharge_Address
        {
            set;
            get;
        }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Recharge_Money
        {
            set;
            get;
        }
        /// <summary>
        /// 充值后可用余额
        /// </summary>
        public decimal Recharge_MoneyLeft
        {
            set;
            get;
        }
        /// <summary>
        /// 充值币种
        /// </summary>
        public int Recharge_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 充值状态(0:待充值1:已充值)
        /// </summary>
        public int Recharge_Type
        {
            set;
            get;
        }
        /// <summary>
        /// 状态(0锁定1正常默认1)
        /// </summary>
        public int Recharge_Status
        {
            set;
            get;
        }
    }
}
