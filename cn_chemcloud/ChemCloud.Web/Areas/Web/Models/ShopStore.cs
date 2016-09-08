using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChemCloud.Model;

namespace ChemCloud.Web.Areas.Web.Models
{
    public class ShopStore
    {
        public ShopStore()
        {
        }

        public string CompanyAddress { get; set; }

        public string CompanyFoundingDate { get; set; }



        public string URL { get; set; }
        public string ContactsEmail
        {
            get;
            set;
        }

        

        public string ContactsPhone
        {
            get;
            set;
        }
      
        public string OrderCounts
        {
            get;
            set;
        }

        public string PackingQuality { get; set; }

        public string OrderHandlingComplaints
        {
            get;
            set;
        }

        public string OrderProductConsultation
        {
            get;
            set;
        }

        public string OrderReplyComments
        {
            get;
            set;
        }

        public string OrderWaitDelivery
        {
            get;
            set;
        }

        public string OrderWaitPay
        {
            get;
            set;
        }

        public string OrderWithRefund
        {
            get;
            set;
        }

        public string OrderWithRefundAndRGoods
        {
            get;
            set;
        }

        public string ProductAndDescription
        {
            get;
            set;
        }

        public string ComprehensiveEvaluation
        {
            get
            {
                double num = (double)(Convert.ToDouble(ProductAndDescription) + Convert.ToDouble(SellerServiceAttitude) + Convert.ToDouble(SellerDeliverySpeed) + Convert.ToDouble(PackingQuality)) / 4;

                return num.ToString();
            }
        }

        public string ProductAndDescriptionPercentage
        {
            get
            {
                int num = (int)(Convert.ToDouble(ProductAndDescription) / 5 * 100);
                return string.Concat(num.ToString(), "%");
            }
        }

        public string PackingQualityPercentage
        {
            get
            {
                int num = (int)(Convert.ToDouble(PackingQuality) / 5 * 100);
                return string.Concat(num.ToString(), "%");
            }
        }

        public string ProductsAuditFailed
        {
            get;
            set;
        }

        public string ProductsBrands
        {
            get;
            set;
        }

        public string ProductsEvaluation
        {
            get;
            set;
        }

        public string ProductsInDraft
        {
            get;
            set;
        }

        public string ProductsInfractionSaleOff
        {
            get;
            set;
        }

        public string ProductsInStock
        {
            get;
            set;
        }

        public string ProductsNumber
        {
            get;
            set;
        }

        public string ProductsNumberIng
        {
            get;
            set;
        }

        public string ProductsOnSale
        {
            get;
            set;
        }

        public string ProductsWaitForAuditing
        {
            get;
            set;
        }

        
        public string SellerDeliverySpeed
        {
            get;
            set;
        }

        public string SellerDeliverySpeedPercentage
        {
            get
            {
                int num = (int)(Convert.ToDouble(SellerDeliverySpeed) / 5 * 100);
                return string.Concat(num.ToString(), "%");
            }
        }

        public string SellerServiceAttitude
        {
            get;
            set;
        }

        public string SellerServiceAttitudePercentage
        {
            get
            {
                int num = (int)(Convert.ToDouble(SellerServiceAttitude) / 5 * 100);
                return string.Concat(num.ToString(), "%");
            }
        }

        public string ShopEndDate
        {
            get;
            set;
        }

        public string ShopGradeName
        {
            get;
            set;
        }

        public long ShopId
        {
            get;
            set;
        }

        public string ShopLogo
        {
            get;
            set;
        }

        public string ShopName
        {
            get;
            set;
        }

        public string UseSpace
        {
            get;
            set;
        }

        public string UseSpaceing
        {
            get;
            set;
        }
        public string Fax
        {
            set;
            get;
        }
        public string GMPPhoto
        {
            set;
            get;
        }
        public string FDAPhoto
        {
            set;
            get;
        }
        public string ISOPhoto
        {
            set;
            get;
        }
        public string OrganizationCodePhoto
        {
            set;
            get;
        }
        public string TaxRegistrationCertificatePhoto
        {
            set;
            get;
        }
        public string BusinessLicenceNumberPhoto
        {
            set;
            get;
        }
        public string ShopAccount
        {
            set;
            get;
        }
        public string ContactsName
        {
            set;
            get;
        }
        public string BankName
        {
            set;
            get;
        }
        public string BankAccountNumber
        {
            set;
            get;
        }
    }
}