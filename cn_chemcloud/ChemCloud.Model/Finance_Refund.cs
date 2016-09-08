using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    /// <summary>
    /// 财务 退款
    /// </summary>
    public class Finance_Refund : BaseModel
    {
        public Finance_Refund() { }
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
        /// 退款单号
        /// </summary>
        public long Refund_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 退款的订单号
        /// </summary>
        public long Refund_OrderNum
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public long Refund_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商用户类型
        /// </summary>
        public int Refund_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Refund_UserName { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal Refund_Money
        {
            set;
            get;
        }
        /// <summary>
        /// 退款币种
        /// </summary>
        public int Refund_MoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 退款手续费
        /// </summary>
        public decimal Refund_SXMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 是否出境(0未出境1已出境默认0)
        /// </summary>
        public int Refund_ISChujing
        {
            set;
            get;
        }
        /// <summary>
        /// 退款IP
        /// </summary>
        public string Refund_Address
        {
            set;
            get;
        }
        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime Refund_Time
        {
            set;
            get;
        }
        /// <summary>
        /// 状态(0未成功退款1同意退款2拒绝退款 默认0)
        /// </summary>
        public int Refund_Status
        {
            set;
            get;
        }
        /// <summary>
        /// 采购商编号
        /// </summary>
        public long Refund_ToUserId { get; set; }
        /// <summary>
        /// 采购商类型
        /// </summary>
        public int Refund_ToUserType { get; set; }
        /// <summary>
        /// 采购商名称
        /// </summary>
        public string Refund_ToUserName { get; set; }
    }
}
