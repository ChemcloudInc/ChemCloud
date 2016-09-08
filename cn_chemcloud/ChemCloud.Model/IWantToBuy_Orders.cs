/* ==============================================================================
* 功能描述：IWantToBuy_Orders  我要采购信息表对应实体类
 
* 创 建 者：An

* 创建日期：2016/8/16 14:32:39
* ==============================================================================*/

using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class IWantToBuy_Orders : BaseModel
    {
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
        /// 采购编号
        /// </summary>
        public Int64 PurchaseNum { get; set; }
        /// <summary>
        /// 我要供应ID
        /// </summary>
        public Int64 IWantToSupplyID { get; set; }
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
        /// 订单状态（4：未支付；5：已支付；6：已发货；7：已签收；）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayDate { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNum { get; set; }
        /// <summary>
        /// 物流类型
        /// </summary>
        public int LogisticsType { get; set; }
        /// <summary>
        /// 物流备注
        /// </summary>
        public string LogisticsDes { get; set; }
    }

    public class Result_IWantToBuy_Orders : BaseModel
    {
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
        /// 采购编号
        /// </summary>
        public string PurchaseNum { get; set; }
        /// <summary>
        /// 我要供应ID
        /// </summary>
        public Int64 IWantToSupplyID { get; set; }
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
        /// 订单状态（4：未支付；5：已支付；6：已发货；7：已签收；）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string PayDate { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNum { get; set; }
        /// <summary>
        /// 物流类型
        /// </summary>
        public int LogisticsType { get; set; }

        /// <summary>
        /// 物流备注
        /// </summary>
        public string LogisticsDes { get; set; }
    }

}
