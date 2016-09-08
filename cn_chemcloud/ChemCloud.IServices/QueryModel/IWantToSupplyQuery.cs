using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class IWantToSupplyQuery
    {
        public Int64 Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public Int64 PurchaseNum { get; set; }
        /// <summary>
        /// 我要采购ID
        /// </summary>
        public Int64 IWantToBuyID { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal UnitPrice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public Decimal TotalPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Double Quantity { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public string unit = string.Empty;
        /// <summary>
        /// 最迟交付日期
        /// </summary>
        public DateTime LatestDeliveryTime { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public Int64 SupplierID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 中标时间
        /// </summary>
        public DateTime BidDate { get; set; }
        /// <summary>
        /// 竞价状态（0：竞价中；1：竞价成功；2：竞价失败；3：放弃竞价；）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 货币类型（1：人民币：￥；2：美元$）
        /// </summary>
        public int TypeOfCurrency { get; set; }
    }

    public class IWantToSupplyModifyQuery : IWantToSupplyQuery
    {
        public long Id { get; set; }

    }
}
