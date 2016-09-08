using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Model
{
    public class OrderPurchasing : BaseModel
    {
        public OrderPurchasing() { }
        /// <summary>
        /// 自动标识
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
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int ProductCount { get; set; }
        /// <summary>
        /// 纯度
        /// </summary>
        public string ProductPurity { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string ProductDesc { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ConUserName { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string ConWebsite { get; set; }
        /// <summary>
        /// 联系电话或手机
        /// </summary>
        public string ConTelPhone { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否锁定 默认0 未锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        [NotMapped]
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public string ProductPrice { get; set; }
        /// <summary>
        /// 代购商回复的消息
        /// </summary>
        public string ReplyContent { get; set; }
        /// <summary>
        /// 支付凭证
        /// </summary>
        public string ZhifuImg { get; set; }
        /// <summary>
        /// 快递类别
        /// </summary>
        public string KuaiDi { get; set; }
        /// <summary>
        /// 快递编号
        /// </summary>
        public string KuaiDiNo { get; set; }


        /// <summary>
        /// mol字符串 marvin
        /// </summary>
        public string Mol { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 支付费用
        /// </summary>
        public decimal Cost { get; set; }

        public string CASNo { get; set; }

        public string ChemName { get; set; }
    }
}
