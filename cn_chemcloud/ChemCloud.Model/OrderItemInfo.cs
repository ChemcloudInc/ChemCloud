using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class OrderItemInfo : BaseModel
    {
        [NotMapped]
        public int Pub_CID { get; set; }

        private long _id;

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

        public virtual ICollection<ProductCommentInfo> ChemCloud_ProductComments
        {
            get;
            set;
        }

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

        public virtual ChemCloud.Model.OrderInfo OrderInfo
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.OrderRefundInfo> OrderRefundInfo
        {
            get;
            set;
        }

        public string ProductCode
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
        public string PackingUnit
        {
            get;
            set;
        }
        public string Purity
        {
            get;
            set;
        }

        public string SpecLevel
        {
            get;
            set;
        }

        public OrderItemInfo()
        {
            OrderRefundInfo = new HashSet<ChemCloud.Model.OrderRefundInfo>();
            ChemCloud_ProductComments = new HashSet<ProductCommentInfo>();
        }

        [NotMapped]
        public string CASNo { get; set; }

        [NotMapped]
        public string MolecularFormula { get; set; }
        [NotMapped]
        public string ImagePath { get; set; }
    }
}