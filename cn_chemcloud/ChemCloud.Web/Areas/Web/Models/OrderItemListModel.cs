using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.Web.Models
{
    public class OrderItemListModel
    {
        public ChemCloud.Model.CashDepositsObligation CashDepositsObligation
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }

        public decimal CommisRate
        {
            get;
            set;
        }

        public decimal CostPrice
        {
            get;
            set;
        }

        public decimal DiscountAmount
        {
            get;
            set;
        }

        public decimal? EnabledRefundAmount
        {
            get;
            set;
        }

        public long Id
        {
            get;
            set;
        }

        public bool IsLimitBuy
        {
            get;
            set;
        }

        public long OrderId
        {
            get;
            set;
        }

        public long ProductId
        {
            get;
            set;
        }

        public string ProductName
        {
            get;
            set;
        }

        public long Quantity
        {
            get;
            set;
        }

        public decimal RealTotalPrice
        {
            get;
            set;
        }

        public decimal RefundPrice
        {
            get;
            set;
        }

        public long ReturnQuantity
        {
            get;
            set;
        }

        public decimal SalePrice
        {
            get;
            set;
        }

        public long ShopId
        {
            get;
            set;
        }

        public string Size
        {
            get;
            set;
        }

        public string SKU
        {
            get;
            set;
        }

        public string SkuId
        {
            get;
            set;
        }

        public string ThumbnailsUrl
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public OrderItemListModel()
        {
        }

        //--包装规格(PackingUnit)、纯度 (Purity)、结构图(STRUCTURE_2D)、CASNo

        /// <summary>
        /// 包装规格
        /// </summary>
        public string PackingUnit
        {
            get;
            set;
        }
        /// <summary>
        /// 纯度
        /// </summary>
        public string Purity
        {
            get;
            set;
        }
        /// <summary>
        /// 结构图
        /// </summary>
        public string STRUCTURE_2D
        {
            get;
            set;
        }
        /// <summary>
        /// CASNo
        /// </summary>
        public string CASNo
        {
            get;
            set;
        }

        public int Pub_CID { get; set; }
        public string ImagePath
        {
            get;
            set;
        }
    }
}