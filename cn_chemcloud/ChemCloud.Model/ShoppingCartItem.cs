using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ShoppingCartItem
    {
        public DateTime AddTime
        {
            get;
            set;
        }

        public long Id
        {
            get;
            set;
        }

        public long ProductId
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public string SkuId
        {
            get;
            set;
        }

        //,PackingUnit
        public string PackingUnit { get; set; }
        //,Purity
        public string Purity { get; set; }
        //,ProductTotalAmount
        public decimal ProductTotalAmount { get; set; }
        //,CoinType
        public string CoinType { get; set; }
        //ShopId
        public long ShopId { get; set; }


        //cartItemId = item.Id,   
        public long cartItemId { get; set; }
        //imgUrl = string.Concat(product.ImagePath, "/1_50.png"),
        public string imgUrl { get; set; }
        //name = product.ProductName,
        public string name { get; set; }
        //productstatus = product.SaleStatus,
        public int productstatus { get; set; }
        //productauditstatus = product.AuditStatus,
        public int productauditstatus { get; set; }
        //Quantity = item.Quantity,
 
        //shopId = shop.Id,
        public long shopId { get; set; }
        //shopName = shop.ShopName,
        public string shopName { get; set; }
        //productcode = product.ProductCode,
        public string productcode { get; set; }

        public string SpecLevel { get; set; }

        //ExpressCompanyName=distributiontype,
        public string ExpressCompanyName { get; set; }
        //Freight=distributioncost
        public decimal Freight { get; set; }
        //ShippingDate=deliverydate,
        public DateTime ShippingDate { get; set; }
        //RegionId=regionid,
        public int RegionId { get; set; }
        //ShippingAddress=deliveryaddress,
        public string ShippingAddress { get; set; }
        //PaymentTypeName=paymode,
        public string PaymentTypeName { get; set; }
        //Insurancefee=Insurancefee,
        public decimal Insurancefee { get; set; }
        //CartNo=orderid
        public long CartNo { get; set; }

        public int InvoiceType { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceContext { get; set; }

        public decimal ShipmentCost { get; set; }

        public string ShipmentValue { get; set; }

        public ShoppingCartItem()
        {
        }
    }
}