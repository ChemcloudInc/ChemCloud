/* ==============================================================================
* 功能描述：IWantToBuy  我要采购信息表对应实体类
 
* 创 建 者：An

* 创建日期：2016/8/16 14:32:39
* ==============================================================================*/

using ChemCloud.Model;
using ChemCloud.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    public class IWantToBuy : BaseModel
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

    public class Result_IWantToBuy : BaseModel
    {
        /// <summary>
        /// 采购编号
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
        public string DeliveryDate { get; set; }
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
        public string CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        
        public string EndDate { get; set; }
        /// <summary>
        /// 采购信息状态（0：公示中；1：废弃采购；2：终止公示；3：已确定；4：已下单；5：已支付；）
        /// </summary>
        public int Status { get; set; }
        public string StatusStr { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 货币类型（1：人民币：￥；2：美元$）
        /// </summary>
        public string TypeOfCurrency { get; set; }

        /// <summary>
        /// 投标信息
        /// </summary>
        public Result_Model<Result_IWantToSupply> SupplyModel { get; set; }

        public List<Result_IWantToSupply> SupplyList { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 店铺名称是否成功
        /// </summary>
        public bool ShopNameIsSuccess { get; set; }

        /// <summary>
        /// 中标人是我（0：竞价中；1：我已中标；2：他人中标）
        /// </summary>
        public int IsMine { get; set; }

        /// <summary>
        /// 中标人ID
        /// </summary>
        public long SupplierID { get; set; }

        /// <summary>
        /// 是否参与竞价
        /// </summary>
        public int WhetherParticipation { get; set; }
    }


    #region 公共：返回 对象 + 提示信息 Result_Model
    /// <summary>
    /// 公共：返回 对象 + 提示信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result_Model_List_IWantToBuy<T> where T : class, new()
    {
        public Result_List<Result_Model<T>> List { get; set; }
        public Result_Model<T> Model { get; set; }
        public Result_Msg Msg { get; set; }

    }
    #endregion
}
