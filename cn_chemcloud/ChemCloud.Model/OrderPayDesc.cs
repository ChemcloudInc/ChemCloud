using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class OrderPayDesc : BaseModel
    {
        public OrderPayDesc() { }
        /// <summary>
        /// 自动编号
        /// </summary>
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
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 应付总额
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 实际支付总额
        /// </summary>
        public decimal RealPrice { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 收货时间
        /// </summary>
        public DateTime ReciveTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否分期
        /// </summary>
        public bool Isfenqi { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 支付币种类型
        /// </summary>
        public int CoinType { get; set; }
    }
}
