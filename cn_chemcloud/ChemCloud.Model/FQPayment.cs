using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class FQPayment : BaseModel
    {
        public FQPayment() { }
        /// <summary>
        /// 编号
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
        /// 订单编号 不为空
        /// </summary>
        public long orderId { get; set; }
        /// <summary>
        /// 订单总价 不为空
        /// </summary>
        public decimal TolPrice { get; set; }
        /// <summary>
        /// 订单实付价格 不为空
        /// </summary>
        public decimal RealPrice { get; set; }
        /// <summary>
        /// 订单剩余价格 不为空
        /// </summary>
        public decimal LeftPrice { get; set; }
        /// <summary>
        /// 订单分期人
        /// </summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 分期支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 订单状态 1已完成分期支付  0未完成分期支付
        /// </summary>
        public int Status { get; set; }
    }
}
