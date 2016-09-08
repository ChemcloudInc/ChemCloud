using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    /// <summary>
    /// 退货处理类
    /// </summary>
    public class TH : BaseModel
    {
        #region MODEL
        public TH() { }

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
        /// 退货单号
        /// </summary>
        public long TH_Number
        {
            set;
            get;
        }
        /// <summary>
        /// 退货订单号
        /// </summary>
        public long TH_OrderNum
        {
            set;
            get;
        }
        /// <summary>
        /// 退货时间
        /// </summary>
        public DateTime TH_Time
        {
            set;
            get;
        }
        /// <summary>
        /// 退货人(采购商)
        /// </summary>
        public long TH_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 退货人名(采购商名)
        /// </summary>
        public string TH_UserName { get; set; }
        /// <summary>
        /// 退货人类型
        /// </summary>
        public int TH_UserType
        {
            set;
            get;
        }
        /// <summary>
        /// 退货名称
        /// </summary>
        public string TH_ProductName
        {
            set;
            get;
        }
        /// <summary>
        /// 退货数量
        /// </summary>
        public int TH_ProductCount
        {
            set;
            get;
        }
        /// <summary>
        /// 退货应该返款
        /// </summary>
        public decimal TH_ProductMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 实际退款
        /// </summary>
        public decimal TH_ProductMoneyReal
        {
            set;
            get;
        }
        /// <summary>
        /// 退款币种
        /// </summary>
        public int TH_ProductMoneyType
        {
            set;
            get;
        }
        /// <summary>
        /// 退款给谁(供应商编号)
        /// </summary>
        public long TH_ToUserId
        {
            set;
            get;
        }
        /// <summary>
        /// 供应商名
        /// </summary>
        public string TH_ToUserName { get; set; }
        /// <summary>
        /// 供应商类型
        /// </summary>
        public int TH_ToUserType
        {
            set;
            get;
        }
        /// <summary>
        /// 退货状态
        /// </summary>
        public int TH_Status
        {
            set;
            get;
        }

        /// <summary>
        /// 退货原因
        /// </summary>
        public string TH_Reason
        {
            set;
            get;
        }

        public string TH_WLGS
        {
            set;
            get;
        }

        public string TH_WLDH
        {
            set;
            get;
        }

        #endregion
    }
}
