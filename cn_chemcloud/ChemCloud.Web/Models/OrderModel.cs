using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Models
{
    public class OrderModel
    {
        public string IconSrc
        {
            get;
            set;
        }
        public long CoinType { get; set; }


        public string CoinTypeName { get; set; }
        public string OrderDate
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public long OrderId
        {
            get;
            set;
        }

        public string OrderStatus
        {
            get;
            set;
        }

        public string PaymentTypeName
        {
            get;
            set;
        }

        public int PlatForm
        {
            get;
            set;
        }

        public string PlatformText
        {
            get;
            set;
        }

        public int? RefundStats
        {
            get;
            set;
        }

        public long ShopId
        {
            get;
            set;
        }

        public string ShopName
        {
            get;
            set;
        }

        public decimal TotalPrice
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


        public string ShipOrderNumber
        {
            get;
            set;
        }
        public string SellerRemark { get; set; }
        public string ExpressCompanyName
        {
            get;
            set;
        }

        public OrderModel()
        {
        }

        public string FinishDate { get; set; }
        //ps:订单列表 产品详细字段
        public string ProductName { get; set; }
        public string CASNo { get; set; }
        public long Quantity { get; set; }
        public string PackingUnit { get; set; }

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

    }
}