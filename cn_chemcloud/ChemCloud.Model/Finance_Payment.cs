using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 支付
    /// </summary>
    public class Finance_Payment : BaseModel
    {
        public Finance_Payment() { }
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
        /// 支付单号
        /// </summary>
        public long PayMent_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long PayMent_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int PayMent_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public long PayMent_OrderNum
        {
            set;
            get;
        }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayMent_PayTime
        {
            set;
            get;
        }
        /// <summary>
        /// 实际支付额
        /// </summary>
        public decimal PayMent_PayMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 应付总额
        /// </summary>
        public decimal PayMent_TotalMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 保险费
        /// </summary>
        public decimal PayMent_BXMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal PayMent_YFMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 交易费
        /// </summary>
        public decimal PayMent_JYMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal PayMent_SXMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 支付的IP
        /// </summary>
        public string PayMent_PayAddress
        {
            set;
            get;
        }
        /// <summary>
        /// 支付的币种类型
        /// </summary>
        public int PayMent_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 支付状态(0未支付1已支付默认1)
        /// </summary>
        public int PayMent_Status
        {
            set;
            get;
        }
        /// <summary>
        /// 支付类型
        /// </summary>
        public int PayMent_Type { get; set; }
    }
}
