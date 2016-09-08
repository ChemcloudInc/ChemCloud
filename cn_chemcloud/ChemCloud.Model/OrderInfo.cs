using ChemCloud.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class OrderInfo : BaseModel
    {
        /// <summary>
        /// 是否物流代发 默认值0  0自发1代发
        /// </summary>
        public string IsBehalfShip { get; set; }

        /// <summary>
        /// 代发物流公司
        /// </summary>
        public string BehalfShipType { get; set; }


        /// <summary>
        /// 代发物流单号
        /// </summary>
        public string BehalfShipNumber { get; set; }

        public int Isreplacedeliver { get; set; }

        public string Replacedeliveraddress { get; set; }

        private long _id;

        public long CoinType { get; set; }

        [NotMapped]
        public string CoinTypeName { get; set; }

        /// <summary>
        /// 是否交保险费0不交1缴纳
        /// </summary>
        public int IsInsurance { get; set; }

        /// <summary>
        /// 保险费
        /// </summary>
        public decimal Insurancefee { get; set; }

        /// <summary>
        /// 交易费
        /// </summary>
        public decimal Transactionfee { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Counterfee { get; set; }

        [NotMapped]
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal OrderTotalAmount
        {
            get
            {
                return ProductTotalAmount + Freight + Insurancefee + Transactionfee + Counterfee;
            }
        }

        public OrderInfo.ActiveTypes ActiveType
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }
        public string orderAddress
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }

        public string CloseReason
        {
            get;
            set;
        }

        [NotMapped]
        public decimal CommisAmount
        {
            get
            {
                return CommisTotalAmount - RefundCommisAmount;
            }
        }

        public decimal CommisTotalAmount
        {
            get;
            set;
        }

        public decimal DiscountAmount
        {
            get;
            set;
        }

        public string ExpressCompanyName
        {
            get;
            set;
        }

        public DateTime? FinishDate
        {
            get;
            set;
        }

        public decimal Freight
        {
            get;
            set;
        }

        public string GatewayOrderId
        {
            get;
            set;
        }

        [NotMapped]
        public bool HaveDelProduct
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

        public decimal IntegralDiscount
        {
            get;
            set;
        }

        public string InvoiceContext
        {
            get;
            set;
        }

        public string InvoiceTitle
        {
            get;
            set;
        }

        public ChemCloud.Model.InvoiceType InvoiceType
        {
            get;
            set;
        }

        public bool IsPrinted
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.OrderCommentInfo> OrderCommentInfo
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.OrderComplaintInfo> OrderComplaintInfo
        {
            get;
            set;
        }

        public DateTime OrderDate
        {
            get;
            set;
        }

        [NotMapped]
        public decimal OrderEnabledRefundAmount
        {
            get
            {
                decimal num = new decimal(0);
                switch (OrderStatus)
                {
                    case OrderInfo.OrderOperateStatus.WaitDelivery:
                        {
                            num = (ProductTotalAmount + Freight) - DiscountAmount;
                            return num;
                        }
                    case OrderInfo.OrderOperateStatus.WaitReceiving:
                    case OrderInfo.OrderOperateStatus.Finish:
                        {
                            num = (ProductTotalAmount - DiscountAmount) - RefundTotalAmount;
                            return num;
                        }
                    case OrderInfo.OrderOperateStatus.Close:
                        {
                            return num;
                        }
                    default:
                        {
                            return num;
                        }
                }
            }
        }

        public virtual ICollection<ChemCloud.Model.OrderItemInfo> OrderItemInfo
        {
            get;
            set;
        }

        public virtual ICollection<ChemCloud.Model.OrderOperationLogInfo> OrderOperationLogInfo
        {
            get;
            set;
        }

        [NotMapped]
        public long OrderProductQuantity
        {
            get
            {
                long quantity = 0;
                foreach (ChemCloud.Model.OrderItemInfo orderItemInfo in OrderItemInfo)
                {
                    quantity = quantity + orderItemInfo.Quantity;
                }
                return quantity;
            }
        }

        [NotMapped]
        public long OrderReturnQuantity
        {
            get
            {
                long returnQuantity = 0;
                foreach (ChemCloud.Model.OrderItemInfo orderItemInfo in OrderItemInfo)
                {
                    returnQuantity = returnQuantity + orderItemInfo.ReturnQuantity;
                }
                return returnQuantity;
            }
        }

        public OrderInfo.OrderOperateStatus OrderStatus
        {
            get;
            set;
        }


        public OrderInfo.OrderTypes? OrderType
        {
            get;
            set;
        }

        public DateTime? PayDate
        {
            get;
            set;
        }

        public string PaymentTypeGateway
        {
            get;
            set;
        }

        public string PaymentTypeName
        {
            get;
            set;
        }

        public string PayRemark
        {
            get;
            set;
        }

        public PlatformType Platform
        {
            get;
            set;
        }

        public decimal ProductTotalAmount
        {
            get;
            set;
        }

        public decimal RefundCommisAmount
        {
            get;
            set;
        }

        [NotMapped]
        public int? RefundStats
        {
            get;
            set;
        }

        public decimal RefundTotalAmount
        {
            get;
            set;
        }

        public string RegionFullName
        {
            get;
            set;
        }
        public string orderRegionFullName
        {
            get;
            set;
        }

        public int? orderRegionId
        {
            get;
            set;
        }

        public int RegionId
        {
            get;
            set;
        }

        public string SellerAddress
        {
            get;
            set;
        }

        public string SellerPhone
        {
            get;
            set;
        }
        /// <summary>
        /// 判断平台是否已经填写完物流单号  默认是空 如果填写 则是"ok"
        /// </summary>
        public string SellerRemark
        {
            get;
            set;
        }

        public string ShipOrderNumber
        {
            get;
            set;
        }

        public DateTime? ShippingDate
        {
            get;
            set;
        }

        public string ShipTo
        {
            get;
            set;
        }

        [NotMapped]
        public decimal ShopAccountAmount
        {
            get
            {
                return (OrderTotalAmount - RefundTotalAmount) - CommisAmount;
            }
        }

        public long ShopId
        {
            get;
            set;
        }
        [NotMapped]
        public string CompanyName
        {
            get;
            set;
        }
        public string ShopName
        {
            get;
            set;
        }

        [NotMapped]
        public string ShowExpressCompanyName
        {
            get
            {
                string expressCompanyName = ExpressCompanyName;
                if (expressCompanyName == "-1")
                {
                    expressCompanyName = "其他";
                }
                return expressCompanyName;
            }
        }

        public decimal Tax
        {
            get;
            set;
        }

        public int TopRegionId
        {
            get;
            set;
        }

        public long UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string UserRemark
        {
            get;
            set;
        }

        public OrderInfo()
        {
            OrderComplaintInfo = new HashSet<ChemCloud.Model.OrderComplaintInfo>();
            OrderItemInfo = new HashSet<ChemCloud.Model.OrderItemInfo>();
            OrderOperationLogInfo = new HashSet<ChemCloud.Model.OrderOperationLogInfo>();
            OrderCommentInfo = new HashSet<ChemCloud.Model.OrderCommentInfo>();
        }

        public enum ActiveTypes
        {
            [Description("无活动")]
            None
        }
        //待支付、已支付、在途中、已签收、已完结
        public enum OrderOperateStatus
        {
            [Description("待支付")]
            WaitPay = 1,
            [Description("已支付")]
            WaitDelivery = 2,
            [Description("已发货")]
            WaitReceiving = 3,
            [Description("已关闭")]
            Close = 4,
            [Description("已完结")]
            Finish = 5,
            [Description("已签收")]
            Singed = 6,
            [Description("分期支付")]
            FQPAY = 7,
            [Description("退货中")]
            THing = 8,
            [Description("已退货")]
            TH = 9,
            [Description("退款中")]
            TKing = 10,
            [Description("已退款")]
            TK = 11,
            [Description("拒绝退款")]
            TKRefuse = 12,
            [Description("拒绝退货")]
            THRefuse = 13
        }

        public enum OrderTypes
        {
            [Description("组合购")]
            Collocation = 1,
            [Description("限时购")]
            LimitBuy = 2
        }
    }
}