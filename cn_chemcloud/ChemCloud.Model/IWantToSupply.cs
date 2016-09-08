/* ==============================================================================
* 功能描述：IWantToSupply  我要采购信息表对应实体类
 
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
    public class IWantToSupply : BaseModel
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
        public string Unit { get; set; }
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
        /// 竞价状态（0：竞价中；1：竞价成功；2：竞价失败；3：放弃竞价；4:已下单；5：已支付；6：已发货；7：已签收）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 货币类型（1：人民币：￥；2：美元$）
        /// </summary>
        public int TypeOfCurrency { get; set; }

    }

    public class Result_IWantToSupply : BaseModel
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
        /// 编号
        /// </summary>
        public string PurchaseNum { get; set; }
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
        public string Unit { get; set; }
        /// <summary>
        /// 最迟交付日期
        /// </summary>
        public string LatestDeliveryTime { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public Int64 SupplierID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateDate { get; set; }
        /// <summary>
        /// 中标时间
        /// </summary>
        public string BidDate { get; set; }
        /// <summary>
        /// 竞价状态（0：竞价中；1：竞价成功；2：竞价失败；3：放弃竞价；）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 竞价状态（0：竞价中；1：竞价成功；2：竞价失败；3：放弃竞价；）
        /// </summary>
        public string StatusStr { get; set; }
        /// <summary>
        /// 货币类型（1：人民币：￥；2：美元$）
        /// </summary>
        public string TypeOfCurrency { get; set; }


        /// <summary>
        /// 店铺名称（供应商）
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 店铺Id（供应商）
        /// </summary>
        public long ShopId { get; set; }
        /// <summary>
        /// 当前竞价是否为当前登录供应商（0：不是；1：是）
        /// </summary>
        public int IsMine { get; set; }

    }


    /// <summary>
    /// 供应商信息
    /// </summary>
    public class SupplierInfo
    {
        /// <summary>
        /// 供应商Id
        /// </summary>
        public Int64 SupplierId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

    }
}
