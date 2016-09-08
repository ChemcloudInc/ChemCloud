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
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemCloud.Web.Models
{
    public class ShopModel
    {
        [NotMapped]
        public int Disabled { get; set; }

        public string Account
        {
            get;
            set;
        }

        public string BankAccountName
        {
            get;
            set;
        }

        public string BankAccountNumber
        {
            get;
            set;
        }

        public string BankCode
        {
            get;
            set;
        }

        public string BankName
        {
            get;
            set;
        }

        public string BankPhoto
        {
            get;
            set;
        }

        public string BankRegionId
        {
            get;
            set;
        }

        public List<CategoryKeyVal> BusinessCategory
        {
            get;
            set;
        }

        public DateTime? BusinessLicenceEnd
        {
            get;
            set;
        }

        public string BusinessLicenceNumber
        {
            get;
            set;
        }

        public string BusinessLicenceNumberPhoto
        {
            get;
            set;
        }

        public string BusinessLicenceRegionId
        {
            get;
            set;
        }

        public DateTime? BusinessLicenceStart
        {
            get;
            set;
        }

        public string BusinessLicenseCert
        {
            get;
            set;
        }

        public string BusinessSphere
        {
            get;
            set;
        }
        /// <summary>
        /// 被拒绝原因
        /// </summary>
        public string RefuseReason
        {
            get;
            set;
        }
        [Required(ErrorMessage = "必须填写公司详细地址")]
        public string CompanyAddress
        {
            get;
            set;
        }

        [DisplayName("员工总数")]
        [Range(1, 2147483647, ErrorMessage = "人数必须为大于0的整数")]
        [Required(ErrorMessage = "必须填写员工总数")]
        public ChemCloud.Model.CompanyEmployeeCount CompanyEmployeeCount
        {
            get;
            set;
        }

        public DateTime? CompanyFoundingDate
        {
            get;
            set;
        }

        [MinLength(5, ErrorMessage = "公司名称不能小于5个字符")]
        [Remote("CheckCompanyName", "ShopProfile", "SellerAdmin", ErrorMessage = "该公司名已存在")]
        [Required(ErrorMessage = "必须填写公司名称")]
        [StringLength(255, ErrorMessage = "最大长度不能超过255")]
        public string CompanyName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须填写公司电话")]
        public string CompanyPhone
        {
            get;
            set;
        }

        public string CompanyRegion
        {
            get;
            set;
        }

        [DataType(DataType.Currency, ErrorMessage = "必须为货币值")]
        [Range(typeof(decimal), "0.00", "10000.00", ErrorMessage = "输入不大于10000")]
        [Required(ErrorMessage = "必须填写注册资金")]
        public decimal CompanyRegisteredCapital
        {
            get;
            set;
        }

        public string ContactsEmail
        {
            get;
            set;
        }

        [Required(ErrorMessage = "必须填写联系人姓名")]
        public string ContactsName
        {
            get;
            set;
        }

        [Phone(ErrorMessage = "电话号码不正确")]
        [Required(ErrorMessage = "必须填写联系人电话")]
        public string ContactsPhone
        {
            get;
            set;
        }

        //[Required(ErrorMessage="有效期为必填项")]
        public string EndDate
        {
            get;
            set;
        }

        public string GeneralTaxpayerPhot
        {
            get;
            set;
        }

        public long Id
        {
            get;
            set;
        }

        public bool IsSelf
        {
            get;
            set;
        }

        public string legalPerson
        {
            get;
            set;
        }

        [MaxLength(20, ErrorMessage = "供应商名称最多20个字符")]
        public string Name
        {
            get;
            set;
        }

        public int NewBankRegionId
        {
            get;
            set;
        }

        public int NewCompanyRegionId
        {
            get;
            set;
        }

        public string OrganizationCode
        {
            get;
            set;
        }

        public string OrganizationCodePhoto
        {
            get;
            set;
        }

        public string OtherCert
        {
            get;
            set;
        }

        public string PayPhoto
        {
            get;
            set;
        }

        public string PayRemark
        {
            get;
            set;
        }

        public string ProductCert
        {
            get;
            set;
        }

        //[Required(ErrorMessage="供应商套餐为必填项")]
        public string ShopGrade
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string TaxpayerId
        {
            get;
            set;
        }

        public string TaxRegistrationCertificate
        {
            get;
            set;
        }

        public string TaxRegistrationCertificatePhoto
        {
            get;
            set;
        }
        /// <summary>
        /// 英文的企业名称
        /// </summary>
        [Remote("CheckCompanyName", "ShopProfile", "SellerAdmin", ErrorMessage = "该公司名已存在")]
        [StringLength(255, ErrorMessage = "最大长度不能超过255")]
        public string ECompanyName
        {
            get;
            set;
        }
        /// <summary>
        /// 英文的联系人名称
        /// </summary>
        public string EContactsName
        {
            get;
            set;
        }
        /// <summary>
        /// 英文的公司详细地址
        /// </summary>
        public string ECompanyAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 公司类型
        /// </summary>
        public string CompanyType
        {
            get;
            set;
        }
        /// <summary>
        /// 邮政编号
        /// </summary>
        public string ZipCode
        {
            get;
            set;
        }
        /// <summary>
        /// 传真
        /// </summary>
        //[Phone(ErrorMessage = "传真号码不正确")]
        //[Required(ErrorMessage = "必须填写公司传真")]
        public string Fax
        {
            get;
            set;
        }

        /// <summary>
        ///  网址
        /// </summary>
        public string URL
        {
            get;
            set;
        }
        /// <summary>
        /// 危化品经营许可证
        /// </summary>
        public string ChemicalsBusinessLicense
        {
            get;
            set;
        }
        /// <summary>
        /// 危化品经营许可证编号
        /// </summary>
        public string ChemNumber
        {
            get;
            set;
        }
        public string Logo
        {
            get;
            set;
        }
        public string GMPPhoto
        {
            get;
            set;
        }
        public string FDAPhoto
        {
            get;
            set;
        }
        public string ISOPhoto
        {
            get;
            set;
        }
        public string BeneficiaryBankName
        {
            get;
            set;
        }
        public string SWiftBic
        {
            get;
            set;
        }
        public string BeneficiaryName
        {
            get;
            set;
        }
        public string BeneficiaryAccountNum
        {
            get;
            set;
        }
        public string CompanysAddress
        {
            get;
            set;
        }

        public string BeneficiaryBankBranchAddress
        {
            get;
            set;
        }
        public string CertificatePhoto
        {
            get;
            set;
        }
        public string AbaRoutingNumber
        {
            get;
            set;
        }
        public DateTime CreateDate
        {
            get;
            set;
        }
        public ShopModel()
        {
        }

        public ShopModel(ShopInfo m)
            : this()
        {
            Id = m.Id;
            Account = m.ShopAccount;
            Name = m.ShopName;
            ShopGradeInfo shopGrade = ServiceHelper.Create<IShopService>().GetShopGrade(m.GradeId);
            ShopGrade = (shopGrade == null ? "" : shopGrade.Name);
            Status = m.ShopStatus.ToDescription();
            EndDate = (m.EndDate.HasValue ? m.EndDate.Value.ToString("yyyy-MM-dd") : "");
            IsSelf = m.IsSelf;
            CompanyName = m.CompanyName;
            NewCompanyRegionId = m.CompanyRegionId;
            CompanyRegion = ServiceHelper.Create<IRegionService>().GetRegionFullName(m.CompanyRegionId, " ");
            CompanyAddress = m.CompanyAddress;
            CompanyPhone = m.CompanyPhone;
            CompanyEmployeeCount = m.CompanyEmployeeCount;
            CompanyRegisteredCapital = m.CompanyRegisteredCapital;
            ContactsName = m.ContactsName;
            ContactsPhone = m.ContactsPhone;
            ContactsEmail = m.ContactsEmail;
            BusinessLicenseCert = m.BusinessLicenseCert;
            ProductCert = m.ProductCert;
            OtherCert = m.OtherCert;
            BusinessLicenceNumber = m.BusinessLicenceNumber;
            BusinessLicenceNumberPhoto = m.BusinessLicenceNumberPhoto;
            BusinessLicenceRegionId = ServiceHelper.Create<IRegionService>().GetRegionFullName(m.BusinessLicenceRegionId, " ");
            BusinessLicenceStart = m.BusinessLicenceStart;
            BusinessLicenceEnd = m.BusinessLicenceEnd;
            BusinessSphere = m.BusinessSphere;
            OrganizationCode = m.OrganizationCode;
            OrganizationCodePhoto = m.OrganizationCodePhoto;
            GeneralTaxpayerPhot = m.GeneralTaxpayerPhot;
            BankAccountName = m.BankAccountName;
            BankAccountNumber = m.BankAccountNumber;
            BankName = m.BankName;
            BankCode = m.BankCode;
            BankRegionId = ServiceHelper.Create<IRegionService>().GetRegionFullName(m.BankRegionId, " ");
            NewBankRegionId = m.BankRegionId;
            BankPhoto = m.BankPhoto;
            TaxRegistrationCertificate = m.TaxRegistrationCertificate;
            TaxpayerId = m.TaxpayerId;
            TaxRegistrationCertificatePhoto = m.TaxRegistrationCertificatePhoto;
            PayPhoto = m.PayPhoto;
            PayRemark = m.PayRemark;
            legalPerson = m.legalPerson;
            RefuseReason = m.RefuseReason;
            ECompanyName = m.ECompanyName;
            ECompanyAddress = m.ECompanyAddress;
            EContactsName = m.EContactsName;
            CompanyType = m.CompanyType;
            ZipCode = m.ZipCode;
            Fax = m.Fax;
            URL = m.URL;
            ChemicalsBusinessLicense = m.ChemicalsBusinessLicense;
            Logo = m.Logo;
            GMPPhoto = m.GMPPhoto;
            FDAPhoto = m.FDAPhoto;
            ISOPhoto = m.ISOPhoto;
            ChemNumber = m.ChemNumber;
            CompanyFoundingDate = new DateTime?((m.CompanyFoundingDate.HasValue ? m.CompanyFoundingDate.Value : DateTime.Now));
            BeneficiaryBankName = m.BeneficiaryBankName;
            SWiftBic = m.SWiftBic;
            BeneficiaryName = m.BeneficiaryName;
            BeneficiaryAccountNum = m.BeneficiaryAccountNum;
            CompanysAddress = m.CompanysAddress;
            BeneficiaryBankBranchAddress = m.BeneficiaryBankBranchAddress;
            CertificatePhoto = m.CertificatePhoto;
            AbaRoutingNumber = m.AbaRoutingNumber;
            CreateDate = m.CreateDate;
        }

        public static implicit operator ShopInfo(ShopModel m)
        {
            ShopInfo shopInfo = new ShopInfo()
            {
                Id = m.Id,
                ShopName = m.Name,
                CompanyName = m.CompanyName,
                GradeId = int.Parse(m.ShopGrade),
                EndDate = new DateTime?(DateTime.Parse(m.EndDate)),
                ShopStatus = (ShopInfo.ShopAuditStatus)int.Parse(m.Status),
                BankRegionId = m.NewBankRegionId,
                CompanyRegionId = m.NewCompanyRegionId

            };
            return shopInfo;
        }
    }
}