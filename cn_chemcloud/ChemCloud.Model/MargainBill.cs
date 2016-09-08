using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class MargainBill : BaseModel
    {
        /// <summary>
        /// 发票类型
        /// </summary>
        [NotMapped]
        public int InvoiceType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [NotMapped]
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        [NotMapped]
        public string InvoiceContext { get; set; }

        /// <summary>
        /// 是否支付保险
        /// </summary>
        public int IsInsurance { get; set; }

        /// <summary>
        /// 保险金额
        /// </summary>
        public decimal Insurancefee { get; set; }

        /// <summary>
        /// 配货周期
        /// </summary>
        public string DistributionCycle { get; set; }

        public long CoinType { get; set; }

        [NotMapped]
        public string CoinTypeName { get; set; }

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

        public int IsDelete { get; set; }

        public DateTime CreateDate { get; set; }

        [NotMapped]
        public string strCreateDate { get; set; }

        public string BillNo { get; set; }

        public decimal TotalAmount { get; set; }

        public string DeliverType { get; set; }

        public DateTime DeliverDate { get; set; }

        [NotMapped]
        public string strDeliverDate { get; set; }

        public string DeliverAddress { get; set; }

        public string PayMode { get; set; }

        public EnumBillStatus BillStatus { get; set; }

        public long MemberId { get; set; }
        [NotMapped]
        public string MemberName { get; set; }

        public long ShopId { get; set; }
        [NotMapped]
        public string ShopName { get; set; }

        [NotMapped]
        public List<MargainBillDetail> _MargainBillDetail { get; set; }

        public decimal DeliverCost { get; set; }

        [NotMapped]
        public int RegionId { get; set; }


        [NotMapped]
        public string BuyerEmail { get; set; }//买家邮箱

        [NotMapped]
        public string CASNo { get; set; }//cas no.

        [NotMapped]
        public int ProudctNum { get; set; }//数量

        [NotMapped]
        public string BuyerMessage { get; set; }//买家留言

        [NotMapped]
        public string MessageReply { get; set; }//回复

        /// <summary>
        /// 电话
        /// </summary>
        [NotMapped]
        public string SellerPhone { get; set; } 

        /// <summary>
        /// 开户行及账号
        /// </summary>
        [NotMapped]
        public string SellerRemark { get; set; } 

        /// <summary>
        /// 地址
        /// </summary>
        [NotMapped]
        public string SellerAddress { get; set; } 

    }

    public class MargainBillDetail : BaseModel
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

        public DateTime CreateDate { get; set; }

        public int IsDelete { get; set; }

        public string BillNo { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Num { get; set; }

        public decimal MarketPrice { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal ShopPirce { get; set; }

        public string Purity { get; set; }

        public string PackingUnit { get; set; }

        public decimal FinalPrice { get; set; }

        public long Bidder { get; set; } //出价人

        [NotMapped]
        public string BidderName { get; set; } //出价人


        public string BuyerMessage { get; set; }//买家留言


        public string MessageReply { get; set; }//回复

        public string CASNo { get; set; }

        public string SpecLevel { get; set; }
    }


    public enum EnumBillStatus
    {
        [Description("已提交")]
        SubmitBargain = 1,
        [Description("议价中")]
        Bargaining = 2,
        [Description("结束议价")]
        BargainOver = 3,
        [Description("议价成功")]
        BargainSucceed = 4,

        [Description("删除议价")]
        BargainDelete = 0
    }
}
