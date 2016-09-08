using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ShoppingCartItemInfo : BaseModel
    {
        private long _id;

        public DateTime AddTime
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

        public virtual UserMemberInfo MemberInfo
        {
            get;
            set;
        }

        public long ProductId
        {
            get;
            set;
        }

        public virtual ChemCloud.Model.ProductInfo ProductInfo
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

        public long UserId
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

        public ShoppingCartItemInfo()
        {
        }
    }
}