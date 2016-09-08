using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class OrderSynthesis : BaseModel
    {

        public OrderSynthesis() { }
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
        public string ProductCount { get; set; }
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
        /// 订单价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ShipOrderNumber { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressConpanyName { get; set; }
        /// <summary>
        /// 平台留言
        /// </summary>
        public string OperatorMessage { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }

        public string ZhifuImg { get; set; }

        /// <summary>
        /// mol字符串 marvin
        /// </summary>
        public string Mol { get; set; }

        /// <summary>
        /// 合成周期
        /// </summary>
        public string SynthesisCycle { get; set; }

        /// <summary>
        /// 合成费用
        /// </summary>
        public decimal SynthesisCost { get; set; }

        public string CASNo { get; set; }

        public string ChemName { get; set; }
    }

    public class OrderSynthesis_Index
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string ProductCount { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string PackingUnit { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public long SellNum { get; set; }
    }
}
