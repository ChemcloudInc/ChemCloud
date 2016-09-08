using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class IWantToBuy_OrdersQuery
    {
        public Int64 Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public Int64 PurchaseNum { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Double Quantity { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public Decimal TotalPrice { get; set; }
        /// <summary>
        /// 交付日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 采购商ID
        /// </summary>
        public Int64 PurchaseID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 采购信息状态（0：公示中；1：废弃采购；2：终止公示；3：已确定；4：已下单；5：已支付；）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 货币类型（1：人民币：￥；2：美元$）
        /// </summary>
        public int TypeOfCurrency { get; set; }


    }

    public class IWantToBuy_OrdersModifyQuery : IWantToBuy_OrdersQuery
    {
        public long Id { get; set; }

    }
 
}
