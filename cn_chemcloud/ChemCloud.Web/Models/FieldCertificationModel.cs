using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Models
{
    public class FieldCertificationModel
    {
        public new long Id
        {
            get;
            set;
            
        }
        public string RefuseReason
        {
            get;
            set;
        }
        public string  TelegraphicMoneyOrder
        {
            get;
            set;
        }
        public string Status
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
        
        public decimal AnnualSales
        {
            get;
            set;
        }
        public string  Certification
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
        public string CompanyName
        {
            get;
            set;
        }
        public string LegalPerson
        {
            get;
            set;
        }
        public string CompanyAddress
        {
            get;
            set;
        }
        public decimal CompanyRegisteredCapital
        {
            get;
            set;
        }
        public FieldCertificationModel()
		{
		}
        public FieldCertificationModel(FieldCertification m):this()
        {
            Id = m.Id;
            Status = m.Status.ToDescription();
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