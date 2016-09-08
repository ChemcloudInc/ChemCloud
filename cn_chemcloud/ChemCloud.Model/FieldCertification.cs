using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class FieldCertification : BaseModel
    {
        private long _id;

        public string TelegraphicMoneyOrder
        {
            get;
            set;
        }
        public FieldCertification.CertificationStatus Status
        {
            get;
            set;
        }
        public string RefuseReason
        {
            get;
            set;
        }
        public DateTime? ApplicationDate
        {
            get;
            set;
        }
        public DateTime? ToAcceptTheDate
        {
            get;
            set;
        }
        public DateTime? CertificationDate
        {
            get;
            set;
        }
        public DateTime? FeedbackDate
        {
            get;
            set;
        }
        public decimal Certificationcost
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

        public decimal AnnualSales
        {
            get;
            set;
        }
        public string Certification
        {
            get;
            set;
        }
        public string EnterpriseHonor
        {
            get;
            set;
        }
        public string ProductDetails
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
        [NotMapped]
        public string LegalPerson
        {
            get;
            set;
        }
        [NotMapped]
        public string CompanyAddress
        {
            get;
            set;
        }
        [NotMapped]
        public decimal CompanyRegisteredCapital
        {
            get;
            set;
        }
        [NotMapped]
        public FieldCertification.CertificationStatus ShowCertificationStatus
        {
            get
            {
                FieldCertification.CertificationStatus certificationStatus = FieldCertification.CertificationStatus.Unusable;
                if (this != null)
                {
                    certificationStatus = Status;
                }
                return certificationStatus;
            }
        }
        public enum CertificationStatus
        {
            [Description("不可用")]
            Unusable = 1,
            [Description("已提交")]
            Submit = 2,
            [Description("已接收")]
            Receive = 3,
            [Description("已付款待审核")]
            PayandWaitAudit = 4,
            [Description("审核通过")]
            Open = 5,
            [Description("已拒绝")]
            Refuse = 6
        }
        public FieldCertification()
        {
        }
        public FieldCertification(FieldCertification m)
            : this()
        {
            Id = m.Id;
            Status = m.Status;
            ApplicationDate = m.ApplicationDate;
            ToAcceptTheDate = m.ToAcceptTheDate;
            CertificationDate = m.CertificationDate;
            FeedbackDate = m.FeedbackDate;
            Certificationcost = m.Certificationcost;
            AnnualSales = m.AnnualSales;
            Certification = m.Certification;
            EnterpriseHonor = m.EnterpriseHonor;
            ProductDetails = m.ProductDetails;
            TelegraphicMoneyOrder = m.TelegraphicMoneyOrder;
            CompanyName = m.CompanyName;
            LegalPerson = m.LegalPerson;
            CompanyAddress = m.CompanyAddress;
            CompanyRegisteredCapital = m.CompanyRegisteredCapital;
            RefuseReason = m.RefuseReason;
        }
    }
}
