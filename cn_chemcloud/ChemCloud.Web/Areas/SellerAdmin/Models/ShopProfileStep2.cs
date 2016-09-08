using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ChemCloud.Web.Areas.SellerAdmin.Models
{
	public class ShopProfileStep2
	{
		[Required(ErrorMessage="必须填写银行开户名")]
		public string BankAccountName
		{
			get;
			set;
		}

		[Required(ErrorMessage="必须填写公司银行账号")]
		public string BankAccountNumber
		{
			get;
			set;
		}

		[Required(ErrorMessage="必须填写支行联行号")]
		public string BankCode
		{
			get;
			set;
		}

		[Required(ErrorMessage="必须填写开户银行支行名称")]
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

		[Required(ErrorMessage="必须填写开户银行所在地")]
		public int BankRegionId
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
        public string AbaRoutingNumber
        {
            get;
            set;
        }

		public ShopProfileStep2()
		{
		}
	}
}